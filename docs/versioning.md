# سیاست نسخه‌گذاری

نسخه فعلی پروژه `0.1.0-alpha.1` است. این project هنوز API پایدار عمومی ندارد و از مدل SemVer 2.0.0 استفاده می‌کند.

## منبع نسخه

نسخه build در `Directory.Build.props` به‌صورت متمرکز تعریف شده است. Git tag هنگام release باید دقیقاً با نسخه release مطابقت داشته باشد.

## نگاشت تغییر به نسخه

| تغییر | نسخه |
|---|---|
| قابلیت backward-compatible جدید | MINOR |
| اصلاح یا refactor سازگار | PATCH |
| تغییر breaking در API/LSP/knowledge schema | MAJOR از نسخه 1.0 به بعد |
| تغییر breaking در 0.x | افزایش MINOR و migration note |
| alpha/beta/rc | pre-release suffix |

## نسخه‌های بعدی پیشنهادی

```text
0.1.0-alpha.1  skeleton و قراردادهای پایه
0.1.0-alpha.2  Roslyn semantic host اولیه
0.1.0-beta.1   اولین ruleهای قابل ارزیابی
0.1.0          اولین feature release قابل استفاده
```

تگ release تغییر داده نمی‌شود. اگر release مشکل داشت، patch جدید ساخته می‌شود.

