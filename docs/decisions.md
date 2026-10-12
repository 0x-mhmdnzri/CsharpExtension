# تصمیم‌های معماری

## ADR-001: موتور semantic اختصاصی

### تصمیم

تمام نتیجه‌گیری‌های مربوط به syntax، symbol، type، compilation، completion و code fix از موتور داخلی پروژه می‌آیند. Roslyn یا language server خارجی authority اجرای افزونه نیست.

### دلیل

تحلیل متنی ساده کافی نیست؛ بنابراین lexer، parser، type system، overload resolution، nullable flow و referenceهای چندپروژه‌ای به‌صورت داخلی ساخته می‌شوند و با corpus نسخه‌های C# 6 تا 15 آزمون می‌گردند.

### پیامد

هزینه توسعه و نگهداری بسیار بیشتر می‌شود، اما مالکیت کامل رفتار، استقلال runtime و امکان کنترل دقیق context و ranking به دست می‌آید.

## ADR-006: عدم وابستگی خارجی در runtime افزونه

### تصمیم

هسته افزونه به package، سرویس ابری، مدل خارجی یا language server شخص ثالث وابسته نیست. کتابخانه‌های استاندارد runtime و APIهای خود VS Code فقط زیرساخت میزبان هستند.

### پیامد

باید metadata reader، parser، semantic index، NuGet index، EF knowledge و protocol adapter داخلی توسعه داده شود. Graphify و Ponytail ابزار توسعه و workflow هستند و بخشی از runtime افزونه نیستند.

## ADR-007: پشتیبانی C# 6 تا 15 با capability matrix

### تصمیم

قابلیت هر version در matrix مستقل ثبت می‌شود و parser/semantic rule بر اساس language version فعال می‌گردد.

### پیامد

هر feature جدید باید positive و negative corpus test برای نسخه‌های قبلی داشته باشد. پیشنهاد featureای که با language version پروژه سازگار نیست، نباید rank شود.

## ADR-008: NuGet و EF به‌عنوان بخشی از semantic context

### تصمیم

PackageReference، assets، assembly metadata و API surface هر package در index پروژه ثبت می‌شود. EF Core و providerهای آن rule و model اختصاصی دارند.

### پیامد

اضافه‌شدن یا حذف package یک event بازسازی incremental index تولید می‌کند و completion/ranking بعدی باید از index جدید استفاده کند.

## ADR-002: language server جدا از VS Code client

### تصمیم

UI و lifecycle در extension و تحلیل در process مستقل قرار می‌گیرد.

### دلیل

crash یا memory pressure در تحلیل نباید editor را از کار بیندازد و امکان تست مستقل server لازم است.

## ADR-003: knowledge pack به‌جای self-modifying code

### تصمیم

به‌روزرسانی دانش با pack امضاشده انجام می‌شود و محتوای crawler در runtime اجرا نمی‌شود.

### دلیل

امنیت، rollback، reproducibility و کنترل کیفیت.

## ADR-004: snapshot و generation برای consistency

### تصمیم

هر درخواست با document، graph و knowledge snapshot مشخص اجرا می‌شود. تعویض snapshot فقط در generation جدید انجام می‌شود.

### دلیل

جلوگیری از ترکیب نتایج نسخه‌های متفاوت و پاسخ stale در زمان تغییر سریع فایل.

## ADR-005: precision مقدم بر recall در ruleهای فعال

### تصمیم

ruleهای performance تا زمانی که precision کافی نداشته باشند، به‌صورت hint کم‌اولویت یا خاموش منتشر می‌شوند.

### دلیل

پیشنهادهای نادرست اعتماد کاربر به کل افزونه را کاهش می‌دهند.

