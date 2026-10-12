# ADR-004: snapshot و generation برای consistency

- Status: accepted
- Date: 2026-10-12

## Decision

هر درخواست با document، graph و knowledge snapshot مشخص اجرا می‌شود. تعویض snapshot فقط در generation جدید انجام می‌شود و پاسخ stale حذف می‌گردد.

