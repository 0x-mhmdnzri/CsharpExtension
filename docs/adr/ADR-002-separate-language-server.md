# ADR-002: language server جدا از VS Code client

- Status: accepted
- Date: 2026-10-12

## Decision

VS Code client فقط UI و transport را مدیریت می‌کند؛ تحلیل در process مستقل .NET اجرا می‌شود.

## Consequences

crash یا memory pressure موتور editor را متوقف نمی‌کند و server مستقل قابل benchmark است.

