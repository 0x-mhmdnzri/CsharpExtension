# ADR-007: capability matrix برای C# 6 تا 15

- Status: accepted
- Date: 2026-10-12

## Decision

هر feature language gate دارد و parser، binder، rule و ranker همان gate را می‌خوانند. C# 15 تا زمان تثبیت specification با feature flag فعال می‌شود.

## Consequences

هر feature باید positive و negative fixture برای نسخه‌های قبلی داشته باشد.

