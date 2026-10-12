# طراحی Rule Engine

## اصل اصلی

Rule باید deterministic، explainable، versioned و قابل اجرای مستقل در corpus باشد. Rule نباید از متن خام Blog یا prompt غیرقابل بازتولید تولید شود.

## Schema پیشنهادی Rule

```json
{
  "id": "memory.boxing.001",
  "version": "1.0.0",
  "status": "active",
  "category": "performance",
  "severity": "warning",
  "confidencePolicy": "high-only",
  "appliesTo": {
    "languages": ["C#"],
    "frameworks": ["net8.0", "net9.0"]
  },
  "trigger": {
    "operationKinds": ["Conversion"],
    "semanticPredicates": ["target-is-object", "source-is-value-type"]
  },
  "preconditions": [
    "inside-loop-or-hot-path",
    "not-explicitly-required-by-api"
  ],
  "transformation": {
    "kind": "code-action",
    "safety": "preview-required"
  },
  "explanation": {
    "runtime": "value type may be boxed",
    "memory": "allocation may occur on managed heap",
    "design": "prefer typed boundary when compatible"
  },
  "references": ["knowledge://clr/boxing/1"],
  "tests": ["boxing-in-loop.cs", "boxing-required-by-api.cs"]
}
```

## انواع Rule

### Correctness

زمانی که rule رفتار نادرست، ناسازگاری قرارداد یا خطای قطعی را تشخیص می‌دهد. این دسته می‌تواند diagnostic سطح بالا تولید کند.

### Design

برای coupling، abstraction، API design، CLS compatibility و dependency direction. به‌صورت پیش‌فرض warning است و code action آن باید preview داشته باشد.

### Performance

برای allocation، GC pressure، copying، synchronization و async overhead. بدون evidence کافی نباید error تولید کند.

## سطوح اطمینان

- `certain`: از semantic model یا compiler نتیجه قطعی دارد.
- `high`: الگو با شرایط runtime شناخته‌شده تطبیق دارد.
- `medium`: احتمال هزینه وجود دارد اما profiler یا مسیر اجرا کامل نیست.
- `low`: فقط hint آموزشی؛ در completion پیش‌فرض نمایش داده نمی‌شود.

## چرخه Rule

`draft → corpus-tested → reviewed → active → deprecated`

هر تغییر رفتاری باید version جدید داشته باشد. حذف rule با deprecation انجام می‌شود تا regression history باقی بماند.

## قرارداد Code Action

Code action باید قبل و بعد، محدوده edit، خطر تغییر و روش بازگشت را ارائه کند. transformationهایی که API عمومی یا قرارداد serialization را تغییر می‌دهند، فقط با تأیید صریح کاربر اجرا می‌شوند.

