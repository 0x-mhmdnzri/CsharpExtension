const vscode = require('vscode');
const fs = require('fs');
const path = require('path');
const cp = require('child_process');

let serverProcess;
let nextRequestId = 1;
let pending = new Map();
let inputBuffer = Buffer.alloc(0);
let statusBar;
let errorTerminal;
const debounceTimers = new Map();
const buildTimers = new Map();
let buildProcess;
let buildDiagnostics;

function setStatus(text, tooltip) {
  if (!statusBar) return;
  statusBar.text = `$(database) DEA: ${text}`;
  statusBar.tooltip = tooltip ?? text;
}

function shellQuote(value) {
  return `'${String(value).replace(/'/g, "'\\''")}'`;
}

function reportError(error) {
  const message = error instanceof Error ? error.stack ?? error.message : String(error);
  setStatus(`Error · ${message.split('\n')[0].slice(0, 80)}`, message);
  if (errorTerminal) {
    errorTerminal.show(true);
    const oneLine = message.replace(/\r?\n/g, '\\n');
    errorTerminal.sendText(`printf '%s\\n' ${shellQuote(`[DEA] ${oneLine}`)}`, true);
  }
}

function send(message) {
  if (!serverProcess || serverProcess.killed) throw new Error('Language server is not running');
  const payload = Buffer.from(JSON.stringify(message.body));
  serverProcess.stdin.write(Buffer.concat([Buffer.from(`Content-Length: ${payload.length}\r\n\r\n`), payload]));
}

function handleOutput(chunk) {
  inputBuffer = Buffer.concat([inputBuffer, chunk]);
  while (true) {
    const separator = inputBuffer.indexOf(Buffer.from('\r\n\r\n'));
    if (separator < 0) return;
    const header = inputBuffer.subarray(0, separator).toString('ascii');
    const match = /^Content-Length:\s*(\d+)$/mi.exec(header);
    if (!match) throw new Error('Invalid language server header');
    const length = Number(match[1]);
    const start = separator + 4;
    if (inputBuffer.length < start + length) return;
    const body = JSON.parse(inputBuffer.subarray(start, start + length).toString('utf8'));
    inputBuffer = inputBuffer.subarray(start + length);
    if (body.method === '$/progress') {
      const value = body.params?.value;
      if (body.params?.token === 'dea-index' && value) setStatus(`Index ${value.percentage ?? 0}% · ${value.message ?? ''}`.trim(), value.message);
      continue;
    }
    if (body.id !== undefined && pending.has(body.id)) {
      const resolve = pending.get(body.id);
      pending.delete(body.id);
      if (body.error) resolve(Promise.reject(new Error(body.error.message ?? JSON.stringify(body.error))));
      else resolve(body.result);
    }
  }
}

function request(method, params) {
  const id = nextRequestId++;
  return new Promise((resolve, reject) => {
    pending.set(id, resolve);
    try { send({ body: { jsonrpc: '2.0', id, method, params } }); }
    catch (error) { pending.delete(id); reject(error); }
  });
}

function findProjectPath(document, settings) {
  const configured = settings.get('projectPath');
  if (configured && fs.existsSync(configured)) return configured;
  const folder = vscode.workspace.getWorkspaceFolder(document.uri);
  if (!folder) return undefined;
  return fs.readdirSync(folder.uri.fsPath).find(name => name.endsWith('.csproj'))
    ? path.join(folder.uri.fsPath, fs.readdirSync(folder.uri.fsPath).find(name => name.endsWith('.csproj')))
    : undefined;
}

function parseBuildDiagnostics(output, cwd) {
  const grouped = new Map();
  const pattern = /^(.*)\((\d+),(\d+)(?:-(\d+),(\d+))?\):\s+(error|warning)\s+([^:]+):\s*(.*)$/i;
  for (const line of output.split(/\r?\n/)) {
    const match = pattern.exec(line.trim());
    if (!match) continue;
    const filePath = path.isAbsolute(match[1]) ? match[1] : path.resolve(cwd, match[1]);
    const uri = vscode.Uri.file(filePath).toString();
    const startLine = Math.max(0, Number(match[2]) - 1);
    const startCharacter = Math.max(0, Number(match[3]) - 1);
    const endLine = Math.max(startLine, Number(match[4] || match[2]) - 1);
    const endCharacter = Number(match[5] || match[3]);
    const severity = match[6].toLowerCase() === 'error'
      ? vscode.DiagnosticSeverity.Error : vscode.DiagnosticSeverity.Warning;
    const diagnostic = new vscode.Diagnostic(new vscode.Range(startLine, startCharacter, endLine, endCharacter),
      `${match[7]}: ${match[8]}`, severity);
    diagnostic.source = 'DEA build';
    if (!grouped.has(uri)) grouped.set(uri, []);
    grouped.get(uri).push(diagnostic);
  }
  return grouped;
}

function scheduleBuild(document, output) {
  const settings = vscode.workspace.getConfiguration('dotnetExpertAdvisor', document.uri);
  if (!settings.get('buildOnType', true)) return;
  const projectPath = findProjectPath(document, settings);
  if (!projectPath) return;
  const uri = document.uri.toString();
  const existing = buildTimers.get(uri);
  if (existing) clearTimeout(existing);
  buildTimers.set(uri, setTimeout(() => {
    buildTimers.delete(uri);
    if (buildProcess) buildProcess.kill();
    buildDiagnostics.clear();
    setStatus('Build…', `Building ${path.basename(projectPath)} in the background.`);
    buildProcess = cp.spawn('dotnet', ['build', projectPath, '--no-restore', '--nologo', '-v:minimal'],
      { cwd: path.dirname(projectPath), stdio: ['ignore', 'pipe', 'pipe'] });
    let outputText = '';
    buildProcess.stdout.on('data', data => { outputText += data.toString(); });
    buildProcess.stderr.on('data', data => { outputText += data.toString(); });
    buildProcess.on('error', error => reportError(error));
    buildProcess.on('close', code => {
      if (buildProcess?.pid !== undefined && buildProcess.pid !== 0) buildProcess = undefined;
      const diagnostics = parseBuildDiagnostics(outputText, path.dirname(projectPath));
      for (const [fileUri, items] of diagnostics) buildDiagnostics.set(vscode.Uri.parse(fileUri), items);
      const count = [...diagnostics.values()].reduce((sum, items) => sum + items.length, 0);
      setStatus(code === 0 ? `Build OK · ${count}` : `Build errors · ${count}`,
        code === 0 ? 'Background build completed.' : 'Background build produced diagnostics.');
      if (code !== 0) output.appendLine(outputText);
    });
  }, Math.max(200, Number(settings.get('buildDebounceMs', 700)) || 700)));
}

function activate(context) {
  const output = vscode.window.createOutputChannel('.NET Expert Advisor');
  buildDiagnostics = vscode.languages.createDiagnosticCollection('dotnet-expert-advisor-build');
  errorTerminal = vscode.window.createTerminal({ name: '.NET Expert Advisor Errors' });
  statusBar = vscode.window.createStatusBarItem(vscode.StatusBarAlignment.Right, 100);
  statusBar.name = '.NET Expert Advisor index';
  statusBar.show();
  setStatus('Ready', 'Semantic index is ready.');
  const start = () => {
    if (serverProcess && !serverProcess.killed) return;
    const rid = process.platform === 'win32' ? 'win-x64' :
      process.platform === 'darwin' ? (process.arch === 'arm64' ? 'osx-arm64' : 'osx-x64') :
      process.arch === 'arm64' ? 'linux-arm64' : 'linux-x64';
    const serverDirectory = path.join(context.extensionPath, 'server', rid);
    const bundledExecutable = path.join(serverDirectory, process.platform === 'win32' ? 'LanguageServer.exe' : 'LanguageServer');
    const serverDll = path.join(serverDirectory, 'LanguageServer.dll');
    const command = fs.existsSync(bundledExecutable) ? bundledExecutable : 'dotnet';
    const args = command === 'dotnet' ? [serverDll, '--stdio'] : ['--stdio'];
    serverProcess = cp.spawn(command, args, { cwd: context.extensionPath, stdio: ['pipe', 'pipe', 'pipe'] });
    serverProcess.stdout.on('data', handleOutput);
    serverProcess.stderr.on('data', data => { const message = data.toString().trim(); if (message) { output.appendLine(message); reportError(message); } });
    serverProcess.on('exit', () => { serverProcess = undefined; });
    setStatus('Indexing…', 'Starting the semantic index.');
    request('initialize', { processId: process.pid, capabilities: {} })
      .then(() => setStatus('Ready', 'Semantic index is ready.'))
      .catch(error => { output.appendLine(String(error)); reportError(error); });
  };
  start();
  const provider = vscode.languages.registerCompletionItemProvider({ language: 'csharp' }, {
    async provideCompletionItems(document, position) {
      const started = Date.now();
      setStatus('Index 0% · starting', 'Scanning project context and resolving Roslyn symbols.');
      try {
        const version = document.version;
        const settings = vscode.workspace.getConfiguration('dotnetExpertAdvisor', document.uri);
        const result = await request('textDocument/completion', {
          textDocument: { uri: document.uri.toString(), text: document.getText(), version },
          projectContext: {
            projectPath: settings.get('projectPath'),
            targetFramework: settings.get('targetFramework'),
            configuration: settings.get('configuration', 'Debug')
          },
          position: { line: position.line, character: position.character }
          , progressToken: 'dea-index'
        });
        if (document.version !== version) return [];
        const items = result?.items ?? [];
        const elapsed = Date.now() - started;
        setStatus(`Index 100% · ${items.length} · ${elapsed}ms`, `Semantic index ready; ${items.length} suggestions in ${elapsed}ms.`);
        return items;
      } catch (error) {
        output.appendLine(String(error));
        reportError(error);
        return [];
      }
    }
  }, '.');
  const hoverProvider = vscode.languages.registerHoverProvider({ language: 'csharp' }, {
    async provideHover(document, position) {
      try {
        const settings = vscode.workspace.getConfiguration('dotnetExpertAdvisor', document.uri);
        return await request('textDocument/hover', { textDocument: { uri: document.uri.toString(), text: document.getText(), version: document.version },
          projectContext: { projectPath: settings.get('projectPath'), targetFramework: settings.get('targetFramework'), configuration: settings.get('configuration', 'Debug') },
          position: { line: position.line, character: position.character } });
      } catch (error) { reportError(error); return undefined; }
    }
  });
  const definitionProvider = vscode.languages.registerDefinitionProvider({ language: 'csharp' }, {
    async provideDefinition(document, position) {
      try {
        const settings = vscode.workspace.getConfiguration('dotnetExpertAdvisor', document.uri);
        return await request('textDocument/definition', { textDocument: { uri: document.uri.toString(), text: document.getText(), version: document.version },
          projectContext: { projectPath: settings.get('projectPath'), targetFramework: settings.get('targetFramework'), configuration: settings.get('configuration', 'Debug') },
          position: { line: position.line, character: position.character } });
      } catch (error) { reportError(error); return []; }
    }
  });
  const changeSubscription = vscode.workspace.onDidChangeTextDocument(event => {
    if (event.document.languageId !== 'csharp' || event.contentChanges.length === 0) return;
    const uri = event.document.uri.toString();
    const existing = debounceTimers.get(uri);
    if (existing) clearTimeout(existing);
    const debounceMs = vscode.workspace.getConfiguration('dotnetExpertAdvisor', event.document.uri)
      .get('completionDebounceMs', 180);
    debounceTimers.set(uri, setTimeout(() => {
      debounceTimers.delete(uri);
      const editor = vscode.window.activeTextEditor;
      if (!editor || editor.document.uri.toString() !== uri || editor.document.version !== event.document.version) return;
      vscode.commands.executeCommand('editor.action.triggerSuggest').catch(error => reportError(error));
    }, Math.max(50, Number(debounceMs) || 180)));
    scheduleBuild(event.document, output);
  });
  context.subscriptions.push(output, provider, hoverProvider, definitionProvider, statusBar, errorTerminal, changeSubscription,
    buildDiagnostics,
    vscode.commands.registerCommand('dotnetExpertAdvisor.startServer', start));
}

function deactivate() {
  if (serverProcess && !serverProcess.killed) serverProcess.kill();
  if (buildProcess) buildProcess.kill();
  pending.clear();
  for (const timer of debounceTimers.values()) clearTimeout(timer);
  debounceTimers.clear();
  for (const timer of buildTimers.values()) clearTimeout(timer);
  buildTimers.clear();
  statusBar = undefined;
  errorTerminal = undefined;
}

module.exports = { activate, deactivate };
