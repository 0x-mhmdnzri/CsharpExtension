const vscode = require('vscode');
const fs = require('fs');
const path = require('path');
const cp = require('child_process');

let serverProcess;
let nextRequestId = 1;
let pending = new Map();
let inputBuffer = Buffer.alloc(0);

function send(message) {
  if (!serverProcess || serverProcess.killed) return Promise.reject(new Error('Language server is not running'));
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
    if (body.id !== undefined && pending.has(body.id)) {
      pending.get(body.id)(body.result);
      pending.delete(body.id);
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

function activate(context) {
  const output = vscode.window.createOutputChannel('.NET Expert Advisor');
  const start = () => {
    if (serverProcess && !serverProcess.killed) return;
    const serverDirectory = path.join(context.extensionPath, 'server');
    const bundledExecutable = path.join(serverDirectory, process.platform === 'win32' ? 'LanguageServer.exe' : 'LanguageServer');
    const serverDll = path.join(serverDirectory, 'LanguageServer.dll');
    const command = fs.existsSync(bundledExecutable) ? bundledExecutable : 'dotnet';
    const args = command === 'dotnet' ? [serverDll, '--stdio'] : ['--stdio'];
    serverProcess = cp.spawn(command, args, { cwd: context.extensionPath, stdio: ['pipe', 'pipe', 'pipe'] });
    serverProcess.stdout.on('data', handleOutput);
    serverProcess.stderr.on('data', data => output.append(data.toString()));
    serverProcess.on('exit', () => { serverProcess = undefined; });
    request('initialize', { processId: process.pid, capabilities: {} }).catch(error => output.appendLine(String(error)));
  };
  start();
  const provider = vscode.languages.registerCompletionItemProvider({ language: 'csharp' }, {
    async provideCompletionItems(document, position) {
      const result = await request('textDocument/completion', {
        textDocument: { uri: document.uri.toString(), text: document.getText() },
        position: { line: position.line, character: position.character }
      });
      return result?.items ?? [];
    }
  }, '.');
  context.subscriptions.push(output, provider, vscode.commands.registerCommand('dotnetExpertAdvisor.startServer', start));
}

function deactivate() {
  if (serverProcess && !serverProcess.killed) serverProcess.kill();
  pending.clear();
}

module.exports = { activate, deactivate };
