const vscode = require('vscode');
const path = require('path');
const cp = require('child_process');

let serverProcess;

function activate(context) {
  const output = vscode.window.createOutputChannel('.NET Expert Advisor');
  const start = () => {
    if (serverProcess && !serverProcess.killed) {
      output.appendLine('Preview server is already running.');
      output.show(true);
      return;
    }

    const serverDll = path.join(context.extensionPath, 'server', 'LanguageServer.dll');
    serverProcess = cp.spawn('dotnet', [serverDll], { cwd: context.extensionPath });
    serverProcess.stdout.on('data', data => output.append(data.toString()));
    serverProcess.stderr.on('data', data => output.append(`[stderr] ${data}`));
    serverProcess.on('exit', (code, signal) => output.appendLine(`Preview server exited: code=${code ?? 'null'} signal=${signal ?? 'none'}`));
    output.appendLine('Started developer preview. Completion/LSP transport is not implemented yet.');
    output.show(true);
  };

  context.subscriptions.push(output);
  context.subscriptions.push(vscode.commands.registerCommand('dotnetExpertAdvisor.startServer', start));
}

function deactivate() {
  if (serverProcess && !serverProcess.killed) serverProcess.kill();
}

module.exports = { activate, deactivate };
