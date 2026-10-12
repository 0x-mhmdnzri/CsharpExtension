# طراحی ارزیابی و کیفیت پیشنهادها

## مجموعه آزمون

corpus باید شامل این گروه‌ها باشد:

- پروژه‌های کوچک آموزشی
- solutionهای چندپروژه‌ای
- ASP.NET Core، Worker، Console، Library و AOT
- نمونه‌های allocation و memory pressure
- نمونه‌های عمدی false positive
- تفاوت‌های net8.0، net9.0 و نسخه‌های بعدی

هر نمونه شامل source، انتظار، rule مجاز، rule ممنوع و توضیح علت است.

## معیارهای اصلی

- Precision: چند پیشنهاد نمایش‌داده‌شده مفید بوده‌اند؟
- Recall: چند مورد قابل تشخیص واقعاً تشخیص داده شده‌اند؟
- False-positive rate: چند هشدار بی‌مورد تولید شده است؟
- Ranking quality: پیشنهاد درست در رتبه چندم ظاهر شده است؟
- Latency: p50، p95 و p99 پاسخ LSP
- Stability: تعداد crash، cancellation و stale response
- Memory: مصرف process سرور در solutionهای مختلف

## آستانه اولیه نسخه آزمایشی

- ruleهای correctness با precision حداقل 0.95
- ruleهای performance با precision حداقل 0.85
- false positive قابل مشاهده کمتر از 0.10 برای ruleهای فعال
- p95 completion فوری کمتر از 150ms در پروژه مرجع
- صفر پاسخ stale در تست همزمانی

این آستانه‌ها پس از اجرای corpus واقعی بازبینی می‌شوند.

## Feedback کاربر

feedback به سه شکل ثبت می‌شود:

- accepted: پیشنهاد پذیرفته شد
- dismissed: پیشنهاد رد شد
- ignored: بدون تعامل باقی ماند

feedback به‌صورت پیش‌فرض شامل source code نیست و فقط شناسه rule، نسخه pack، نوع پروژه و نتیجه تعامل ذخیره می‌شود. استفاده از آن برای ranking باید versioned و قابل خاموش‌کردن باشد.

## تست‌های لازم

- unit test برای predicateهای Roslyn
- golden test برای edit و diagnostic
- integration test برای lifecycle LSP
- compatibility test برای target framework
- performance benchmark برای completion و graph update
- security test برای pack signature و parser
- regression test قبل از هر انتشار knowledge pack

