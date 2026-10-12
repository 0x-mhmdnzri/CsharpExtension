# تصمیم‌های معماری

## ADR-001: استفاده از Roslyn به‌عنوان semantic authority

### تصمیم

تمام نتیجه‌گیری‌های مربوط به syntax، symbol، type، compilation و code fix از Roslyn می‌آیند.

### دلیل

تحلیل متنی به‌تنهایی generic constraint، overload resolution، nullable flow و referenceهای چندپروژه‌ای را دقیق تشخیص نمی‌دهد.

### پیامد

مدیریت نسخه Roslyn، workspace و مصرف حافظه پیچیده‌تر می‌شود، اما دقت و قابلیت بازتولید افزایش می‌یابد.

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

