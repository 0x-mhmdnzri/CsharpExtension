# قرارداد LSP و مدل پاسخ پیشنهادها

## هدف

Language Server اختصاصی باید رفتار قابل پیش‌بینی، قابل لغو و قابل اندازه‌گیری داشته باشد. Roslyn منبع تحلیل است و LSP قرارداد ارتباط با VS Code است.

## چرخه عمر

1. `initialize`: اعلام نسخه پروتکل، قابلیت‌ها، نسخه server و شناسه knowledge pack.
2. `initialized`: شروع بارگذاری workspace.
3. `workspace/didChangeWorkspaceFolders`: بازسازی context پروژه‌های تغییرکرده.
4. `textDocument/didOpen` و `didChange`: به‌روزرسانی incremental.
5. `shutdown` و `exit`: آزادسازی compilation، graph و cache.

تمام عملیات تحلیلی باید cancellation token داشته باشند و نتیجه قدیمی نباید نتیجه جدیدتر را overwrite کند.

## قابلیت‌های پایه

- `textDocument/completion`
- `completionItem/resolve`
- `textDocument/hover`
- `textDocument/signatureHelp`
- `textDocument/publishDiagnostics`
- `textDocument/codeAction`
- `textDocument/definition`
- `textDocument/references`
- `textDocument/rename`
- `workspace/symbol`

## قابلیت اختصاصی

### `runtime/explainSuggestion`

پارامتر:

```json
{
  "documentUri": "file:///src/Orders/OrderService.cs",
  "suggestionId": "memory.boxing.001",
  "position": { "line": 42, "character": 18 }
}
```

پاسخ:

```json
{
  "suggestionId": "memory.boxing.001",
  "title": "جلوگیری از boxing در مسیر پرتکرار",
  "category": "performance",
  "confidence": 0.94,
  "evidence": [
    { "kind": "symbol", "value": "OrderId" },
    { "kind": "operation", "value": "Conversion" },
    { "kind": "project", "value": "Orders.Api" }
  ],
  "runtimeImpact": "ایجاد allocation برای تبدیل value type به object",
  "memoryImpact": "احتمال افزایش allocation در مسیر پرتکرار",
  "references": ["knowledge://clr/boxing/1"],
  "limitations": ["نتیجه profiler در این مسیر موجود نیست"]
}
```

## قرارداد Completion

هر `CompletionItem` تولیدشده توسط موتور باید metadata داخلی داشته باشد:

```json
{
  "label": "ValueTask",
  "kind": 7,
  "sortText": "00042",
  "detail": "runtime-aware suggestion",
  "data": {
    "suggestionId": "async.valuetask.002",
    "ruleId": "async.valuetask.002",
    "confidence": 0.88,
    "source": "knowledge://dotnet/blog/2026/example"
  }
}
```

Ranking نهایی بین completion پایه Roslyn و completionهای تحلیلی واحد است. duplicateها بر اساس symbol، edit range و semantic intent حذف می‌شوند.

## زمان‌بندی و سطح تحلیل

- سطح فوری: syntax، symbol و completion پایه؛ هدف p95 کمتر از 150ms
- سطح تعاملی: ruleهای محلی؛ هدف p95 کمتر از 500ms
- سطح عمیق: graph و runtime model؛ نتیجه به‌صورت publish diagnostic یا code action تکمیلی

اگر سطح فوری آماده باشد، server برای تکمیل سطح عمیق منتظر نمی‌ماند.

