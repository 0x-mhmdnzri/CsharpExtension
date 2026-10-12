# قرارداد دقت پیشنهاد

## SuggestionEvidence

```csharp
public sealed record SuggestionEvidence(
    string TopicId,
    EvidenceKind Kind,
    string Subject,
    string? Location,
    string Explanation,
    string SourceId);
```

## SuggestionContract

```csharp
public sealed record SuggestionContract(
    string Id,
    string RuleId,
    ImmutableArray<string> TopicIds,
    LanguageVersion LanguageVersion,
    TargetFrameworkId TargetFramework,
    ImmutableArray<SuggestionEvidence> Evidence,
    VerificationResult Verification,
    double Confidence,
    string Explanation);
```

## قواعد اجباری

- `Verification.Compiles` برای suggestionهای semantic باید true باشد.
- diagnosticهای baseline نباید به‌عنوان خطای پیشنهاد شمرده شوند.
- nullable warning جدید، مگر در حالت آموزشی صریح، پیشنهاد را حذف یا تنزل می‌دهد.
- topicهای نامرتبط نباید فقط برای پرکردن explanation اضافه شوند.
- APIهای obsolete، experimental یا preview باید در ranking و توضیح مشخص شوند.
- C# 15 فقط با feature flag و منبع versioned فعال می‌شود.
- API سطح compile-time از reference assembly و assets واقعی پروژه می‌آید.
- هر rule مثبت و منفی fixture دارد.

## Quality gates

```text
Parser round-trip                 = 100% روی corpus تثبیت‌شده
Differential symbol agreement     = ثبت و بررسی هر اختلاف
Speculative compile-valid rate    >= 99%
False-positive فعال performance   <= 10%
هر Knowledge Atom                 = reviewed + positive/negative fixture
```

## منابع دانشی

- CLR via C# برای runtime، type system، metadata، GC و threading
- C# Language Specification برای grammar، binding و overload resolution
- ECMA-335 برای metadata، IL و CLI behavior
- compiler construction برای lexer، parser، binder و incremental queries
- منابع رسمی .NET/C#/EF برای API، release و idiom

منابع کتابی به concept و claim تبدیل می‌شوند؛ متن کامل منبع در runtime بازتولید نمی‌شود.

