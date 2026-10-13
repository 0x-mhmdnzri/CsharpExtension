# Product Backlog

## Ø±ÙˆØ´ Ø§ÙˆÙ„ÙˆÛŒØªâ€ŒØ¨Ù†Ø¯ÛŒ

- `P0`: Ù¾ÛŒØ´â€ŒÙ†ÛŒØ§Ø² architecture ÛŒØ§ correctnessØ› Ø¨Ø¯ÙˆÙ† Ø¢Ù† feature Ø¨Ø¹Ø¯ÛŒ Ù…Ø¹ØªØ¨Ø± Ù†ÛŒØ³Øª.
- `P1`: Ù‚Ø§Ø¨Ù„ÛŒØª Ù„Ø§Ø²Ù… Ø¨Ø±Ø§ÛŒ MVP Ø¯Ù‚ÛŒÙ‚.
- `P2`: Ù‚Ø§Ø¨Ù„ÛŒØª Ù…Ù‡Ù… Ø¨Ø¹Ø¯ Ø§Ø² MVP.
- `P3`: Ø¨Ù‡Ø¨ÙˆØ¯ ÛŒØ§ Ù‚Ø§Ø¨Ù„ÛŒØª Ø§Ø®ØªÛŒØ§Ø±ÛŒ.

Ù‡Ø± item Ø¨Ø§ÛŒØ¯ ÛŒÚ© branchØŒ acceptance criteria Ùˆ validation Ø¯Ø§Ø´ØªÙ‡ Ø¨Ø§Ø´Ø¯. itemÙ‡Ø§ÛŒ P0 Ùˆ P1 Ø¨Ø§ ÛŒÚ© commit Ø¨Ø²Ø±Ú¯ Ù¾ÛŒØ§Ø¯Ù‡â€ŒØ³Ø§Ø²ÛŒ Ù†Ù…ÛŒâ€ŒØ´ÙˆÙ†Ø¯Ø› Ù‡Ø± Ø¨Ø®Ø´ Ø¨Ø§ÛŒØ¯ Ú©ÙˆÚ†Ú© Ùˆ Ù‚Ø§Ø¨Ù„ bisect Ø¨Ø§Ø´Ø¯.

## ÙˆØ¶Ø¹ÛŒØª ÙØ¹Ù„ÛŒ

| ID | Ø¹Ù†ÙˆØ§Ù† | Priority | Status | Dependency | Branch |
|---|---|---:|---|---|---|
| EF-005 | provenance/confidence Ùˆ IsComplete | P1 | ready | EF-001 | feature/ef/provenance |
| KNOW-006 | concept registry Ø¨Ø±Ø§ÛŒ CLR/C# | P1 | ready | GOV-001 | feature/knowledge/curriculum |

## Ù…Ø³ÛŒØ± MVP Ø¯Ù‚Øªâ€ŒÙ…Ø­ÙˆØ±

```text
LANG-001 â†’ LANG-002 â†’ LANG-003
                         â†“
META-001 â†’ BIND-001 â†’ BIND-002 â†’ BIND-003 â†’ BIND-005
                                      â†“
                               IDE-002 + QA-001
                                      â†“
                                  EF-005
```

MVP Ø¨Ø§ÛŒØ¯ Ø§Ø¨ØªØ¯Ø§ subset Ù…Ø´Ø®Øµ C# 10+ Ùˆ .NET 8 Ø±Ø§ Ø¨Ø§ compile-valid rate Ø¨Ø§Ù„Ø§ Ù¾ÙˆØ´Ø´ Ø¯Ù‡Ø¯Ø› Ø³Ù¾Ø³ capability matrix Ø¨Ù‡ C# 6 ØªØ§ 15 Ú¯Ø³ØªØ±Ø´ Ù…ÛŒâ€ŒÛŒØ§Ø¨Ø¯.

## Backlog ØªÙØµÛŒÙ„ÛŒ

### Compiler core

| ID | Ø®Ø±ÙˆØ¬ÛŒ | Acceptance criteria | Validation |
|---|---|---|---|
| BIND-005 | overload resolution | candidate rejection reason Ùˆ best member Ø¯Ø±Ø³Øª Ø¨Ø§Ø´Ø¯ | spec + differential CI |
| BIND-006 | generic inference Ùˆ LINQ | Ø²Ù†Ø¬ÛŒØ±Ù‡â€ŒÙ‡Ø§ÛŒ Select/Where Ù†ÙˆØ¹ Ø¯Ø±Ø³Øª Ø¯Ø§Ø´ØªÙ‡ Ø¨Ø§Ø´Ù†Ø¯ | LINQ corpus |
| BIND-007 | speculative verification | diagnostic Ø¬Ø¯ÛŒØ¯ Ù†Ø³Ø¨Øª Ø¨Ù‡ baseline ØªØ´Ø®ÛŒØµ Ø¯Ø§Ø¯Ù‡ Ø´ÙˆØ¯ | compile-valid benchmark |

### Metadata Ùˆ project index

| ID | Ø®Ø±ÙˆØ¬ÛŒ | Acceptance criteria | Validation |
|---|---|---|---|

### EF Ùˆ rule knowledge

| ID | Ø®Ø±ÙˆØ¬ÛŒ | Acceptance criteria | Validation |
|---|---|---|---|
| EF-005 | provenance Ùˆ IsComplete | Ù…Ø¯Ù„ Ù†Ø§Ù‚Øµ Ù‡ÛŒÚ† negative rule Ù‚Ø·Ø¹ÛŒ ØªÙˆÙ„ÛŒØ¯ Ù†Ú©Ù†Ø¯ | model corpus |
| EF-006 | conventions/annotations | key/FK/navigation Ø¨Ø§ Ù…Ø¯Ù„ ÙˆØ§Ù‚Ø¹ÛŒ Ù…Ù†Ø·Ø¨Ù‚ Ø¨Ø§Ø´Ø¯ | ToDebugString oracle |
| EF-007 | Fluent interpreter | symbol identity Ùˆ lambda property resolve Ø´ÙˆØ¯ | EF fixture |
| EF-009 | translation matrix | verdict Ø¨Ø± Ø§Ø³Ø§Ø³ provider/version Ø¨Ø§Ø´Ø¯ | ToQueryString CI |
| EF-010 | query completion | Include/ThenInclude Ùˆ IQueryable context Ù…Ø¹ØªØ¨Ø± Ø¨Ø§Ø´Ù†Ø¯ | compile-valid rate |
| KNOW-004 | signed rule pack | hash/signatureØŒ compatibility Ùˆ rollback Ú©Ø§Ø± Ú©Ù†Ø¯ | package tests |

### Editor Ùˆ Ú©ÛŒÙÛŒØª

| ID | Ø®Ø±ÙˆØ¬ÛŒ | Acceptance criteria | Validation |
|---|---|---|---|
| QA-001 | corpus ÙˆØ§Ù‚Ø¹ÛŒ | Ù…Ø¬ÙˆØ²ØŒ Ù†Ø³Ø®Ù‡ Ø²Ø¨Ø§Ù† Ùˆ TFM Ø«Ø¨Øª Ø´Ø¯Ù‡ Ø¨Ø§Ø´Ø¯ | corpus manifest |
| QA-002 | metric harness | Top-kØŒ MRRØŒ compile-valid Ùˆ latency Ø«Ø¨Øª Ø´ÙˆÙ†Ø¯ | benchmark run |
| REL-002 | alpha release | buildØŒ packØŒ changelog Ùˆ tag immutable Ø¨Ø§Ø´Ù†Ø¯ | release checklist |

## Definition of Ready

- contract Ùˆ dependency Ù…Ø´Ø®Øµ Ø§Ø³Øª.
- test fixture ÛŒØ§ Ø±ÙˆØ´ Ø³Ø§Ø®Øª fixture Ù…Ø´Ø®Øµ Ø§Ø³Øª.
- branch name Ùˆ Ù†ÙˆØ¹ commit ØªØ¹ÛŒÛŒÙ† Ø´Ø¯Ù‡ Ø§Ø³Øª.
- Ù…Ø¹Ù„ÙˆÙ… Ø§Ø³Øª Ú©Ù‡ ØªØºÛŒÛŒØ± external dependency ÙˆØ§Ø±Ø¯ runtime Ù…ÛŒâ€ŒÚ©Ù†Ø¯ ÛŒØ§ Ù†Ù‡.

## Definition of Done

- acceptance criteria Ú©Ø§Ù…Ù„ Ø§Ø³Øª.
- build Ùˆ test scope Ø³Ø¨Ø² Ø§Ø³Øª.
- Graphify update Ø§Ù†Ø¬Ø§Ù… Ø´Ø¯Ù‡ Ø§Ø³Øª.
- docs/task state Ø¨Ù‡â€ŒØ±ÙˆØ² Ø§Ø³Øª.
- commit Conventional Ùˆ branch Ù‚Ø§Ø¨Ù„ review Ø§Ø³Øª.
- risk Ùˆ shortcut Ø§Ø­ØªÙ…Ø§Ù„ÛŒ Ø«Ø¨Øª Ø´Ø¯Ù‡ Ø§Ø³Øª.

