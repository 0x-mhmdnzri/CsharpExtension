# ADR-008: NuGet و EF در semantic context

- Status: accepted
- Date: 2026-10-12

## Decision

PackageReference، assets، assembly metadata، EF model و provider/version بخشی از semantic context هستند. اضافه یا حذف package یک incremental index event ایجاد می‌کند.

## Consequences

completion و ranking بدون package index معتبر اجازه پیشنهاد API حدسی ندارند. EF negative rule در مدل ناقص فقط Unknown است.

