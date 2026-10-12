# ADR-006: عدم وابستگی خارجی در runtime

- Status: accepted
- Date: 2026-10-12

## Decision

هسته افزونه به package تحلیل کد، language server، cloud service یا model خارجی وابسته نیست. BCL و API رسمی VS Code زیرساخت میزبان محسوب می‌شوند.

## Consequences

metadata reader، parser، semantic index، NuGet index، EF interpreter و protocol adapter باید داخلی باشند.

