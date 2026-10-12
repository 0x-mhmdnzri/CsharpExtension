# Schemaهای داده

## شناسه‌ها

همه شناسه‌های persistence با رشته‌های دارای prefix ذخیره می‌شوند:

```text
solution:Orders
project:Orders.Api
document:file:///src/Orders.cs
symbol:T:Orders.Order
rule:memory.boxing.001
concept:clr.boxing
```

شناسه باید مستقل از شماره خط باشد. مکان متن فقط به‌عنوان `DocumentSpan` ذخیره می‌شود.

## GraphNode

```json
{
  "id": "symbol:T:Orders.Order",
  "kind": "type",
  "name": "Order",
  "displayName": "Orders.Order",
  "projectId": "project:Orders.Domain",
  "documentUri": "file:///src/Orders.cs",
  "span": { "start": 120, "length": 45 },
  "symbolKey": "T:Orders.Order",
  "frameworks": ["net8.0", "net9.0"],
  "version": 17
}
```

## GraphEdge

```json
{
  "id": "edge:call:method-a:method-b:17",
  "source": "symbol:M:Orders.OrderService.Create",
  "target": "symbol:M:Orders.Order.Order",
  "kind": "constructs",
  "confidence": 1.0,
  "documentVersion": 17
}
```

## RuleDescriptor

```json
{
  "id": "memory.boxing.001",
  "version": "1.0.0",
  "category": "performance",
  "severity": "warning",
  "enabledByDefault": true,
  "minimumConfidence": 0.85,
  "frameworks": ["net8.0", "net9.0"],
  "references": ["concept:clr.boxing"],
  "implementation": "RoslynOperationRule",
  "testCorpus": ["boxing-in-loop", "boxing-required-api"]
}
```

## KnowledgeReference

```json
{
  "id": "knowledge://clr/boxing/1",
  "kind": "concept",
  "title": "Boxing و تخصیص managed heap",
  "source": {
    "type": "book-summary",
    "identifier": "clr-via-csharp",
    "version": "approved-notes-1"
  },
  "claims": ["claim:boxing:allocation-possible"],
  "limitations": ["JIT و context اجرا می‌تواند رفتار را تغییر دهد"]
}
```

## Settings

```json
{
  "analysis.mode": "balanced",
  "analysis.targetFrameworks": ["net8.0"],
  "rules.disabled": [],
  "rules.minimumConfidence": 0.8,
  "knowledge.autoUpdate": true,
  "telemetry.enabled": false,
  "backend.enabled": false
}
```

## Compatibility

هر snapshot شامل `schemaVersion` است. server باید یک major قبلی را برای migration بخواند و pack ناسازگار را فعال نکند.

