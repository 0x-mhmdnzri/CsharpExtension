# ADR-005: precision مقدم بر recall

- Status: accepted
- Date: 2026-10-12

## Decision

Ruleهای correctness و performance تا زمان داشتن evidence و compile verification نمایش داده نمی‌شوند. candidate معتبر اما کم‌اطمینان می‌تواند در حالت آموزشی قابل مشاهده باشد، اما در ranking پیش‌فرض اثر منفی ندارد.

## Consequences

False positive کم می‌شود، اما recall اولیه ممکن است پایین‌تر باشد و corpus بیشتری لازم است.

