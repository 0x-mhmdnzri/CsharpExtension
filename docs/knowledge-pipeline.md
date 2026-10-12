# طراحی Knowledge Pipeline

## منابع

منابع به چهار سطح تقسیم می‌شوند:

1. منابع رسمی Microsoft و مستندات API
2. مقالات رسمی DevBlogs و .NET Blog
3. کتاب‌های دارای مجوز و یادداشت‌های خلاصه‌سازی‌شده
4. نمونه‌های تستی تولیدشده داخل repository

متن کامل منابع کتابی یا Blog در runtime کپی نمی‌شود. خروجی pipeline، claimها، conceptها، ارجاع، مثال کوتاه و rule metadata است.

## مراحل ingestion

```text
Discover
  → Fetch
  → Validate robots/terms
  → Canonicalize
  → Extract sections
  → Deduplicate
  → Create concepts/claims
  → Propose rules
  → Validate against corpus
  → Build pack
  → Sign and publish
```

## شناسه و نسخه

هر منبع، claim و concept شناسه پایدار دارد. تغییر متن منبع با hash و تاریخ ثبت می‌شود. knowledge pack از SemVer پیروی می‌کند:

- patch: اصلاح metadata یا typo
- minor: concept یا rule سازگار جدید
- major: تغییر ranking، حذف rule یا تغییر قرارداد

## Manifest

```json
{
  "packId": "dotnet-runtime-core",
  "version": "0.1.0",
  "createdAt": "2026-10-12T00:00:00Z",
  "minServerVersion": "0.1.0",
  "sources": 42,
  "rules": 18,
  "sha256": "...",
  "signature": "..."
}
```

Client فقط packهایی را می‌پذیرد که signature و compatibility آن‌ها معتبر باشد. دانلود ناقص، rollback و نگهداری آخرین pack سالم الزامی است.

## Blog crawler

- اولویت با RSS و sitemap رسمی است.
- درخواست‌ها rate-limited و cacheable هستند.
- canonical URL و تاریخ انتشار ثبت می‌شود.
- محتوای حذف‌شده از منبع، فوراً از client حذف نمی‌شود؛ ابتدا deprecation منتشر می‌شود.
- parser باید در برابر HTML تغییرکرده، redirect و محتوای مخرب مقاوم باشد.

## تولید Rule

مدل استخراج می‌تواند rule پیشنهادی بسازد، اما rule فعال فقط پس از تولید test case، بررسی محدودیت‌ها و اجرای corpus وارد pack می‌شود. هیچ محتوای خارجی مستقیماً در process language server اجرا نمی‌شود.

