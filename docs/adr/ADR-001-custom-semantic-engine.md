# ADR-001: semantic engine اختصاصی

- Status: accepted
- Date: 2026-10-12

## Decision

Parser، binder، type system، flow analysis، completion و ranking توسط همین پروژه پیاده‌سازی می‌شوند. Roslyn و language serverهای خارجی فقط در CI به‌عنوان oracle مجازند.

## Consequences

هزینه توسعه بالاست، اما رفتار runtime مستقل و قابل کنترل است. Symbol dump و differential testing برای کاهش ریسک الزامی است.

