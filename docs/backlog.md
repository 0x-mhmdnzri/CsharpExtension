# Product Backlog

## Ã˜Â±Ã™Ë†Ã˜Â´ Ã˜Â§Ã™Ë†Ã™â€žÃ™Ë†Ã›Å’Ã˜ÂªÃ¢â‚¬Å’Ã˜Â¨Ã™â€ Ã˜Â¯Ã›Å’

- `P0`: Ã™Â¾Ã›Å’Ã˜Â´Ã¢â‚¬Å’Ã™â€ Ã›Å’Ã˜Â§Ã˜Â² architecture Ã›Å’Ã˜Â§ correctnessÃ˜â€º Ã˜Â¨Ã˜Â¯Ã™Ë†Ã™â€  Ã˜Â¢Ã™â€  feature Ã˜Â¨Ã˜Â¹Ã˜Â¯Ã›Å’ Ã™â€¦Ã˜Â¹Ã˜ÂªÃ˜Â¨Ã˜Â± Ã™â€ Ã›Å’Ã˜Â³Ã˜Âª.
- `P1`: Ã™â€šÃ˜Â§Ã˜Â¨Ã™â€žÃ›Å’Ã˜Âª Ã™â€žÃ˜Â§Ã˜Â²Ã™â€¦ Ã˜Â¨Ã˜Â±Ã˜Â§Ã›Å’ MVP Ã˜Â¯Ã™â€šÃ›Å’Ã™â€š.
- `P2`: Ã™â€šÃ˜Â§Ã˜Â¨Ã™â€žÃ›Å’Ã˜Âª Ã™â€¦Ã™â€¡Ã™â€¦ Ã˜Â¨Ã˜Â¹Ã˜Â¯ Ã˜Â§Ã˜Â² MVP.
- `P3`: Ã˜Â¨Ã™â€¡Ã˜Â¨Ã™Ë†Ã˜Â¯ Ã›Å’Ã˜Â§ Ã™â€šÃ˜Â§Ã˜Â¨Ã™â€žÃ›Å’Ã˜Âª Ã˜Â§Ã˜Â®Ã˜ÂªÃ›Å’Ã˜Â§Ã˜Â±Ã›Å’.

Ã™â€¡Ã˜Â± item Ã˜Â¨Ã˜Â§Ã›Å’Ã˜Â¯ Ã›Å’ÃšÂ© branchÃ˜Å’ acceptance criteria Ã™Ë† validation Ã˜Â¯Ã˜Â§Ã˜Â´Ã˜ÂªÃ™â€¡ Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯. itemÃ™â€¡Ã˜Â§Ã›Å’ P0 Ã™Ë† P1 Ã˜Â¨Ã˜Â§ Ã›Å’ÃšÂ© commit Ã˜Â¨Ã˜Â²Ã˜Â±ÃšÂ¯ Ã™Â¾Ã›Å’Ã˜Â§Ã˜Â¯Ã™â€¡Ã¢â‚¬Å’Ã˜Â³Ã˜Â§Ã˜Â²Ã›Å’ Ã™â€ Ã™â€¦Ã›Å’Ã¢â‚¬Å’Ã˜Â´Ã™Ë†Ã™â€ Ã˜Â¯Ã˜â€º Ã™â€¡Ã˜Â± Ã˜Â¨Ã˜Â®Ã˜Â´ Ã˜Â¨Ã˜Â§Ã›Å’Ã˜Â¯ ÃšÂ©Ã™Ë†Ãšâ€ ÃšÂ© Ã™Ë† Ã™â€šÃ˜Â§Ã˜Â¨Ã™â€ž bisect Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯.

## Ã™Ë†Ã˜Â¶Ã˜Â¹Ã›Å’Ã˜Âª Ã™ÂÃ˜Â¹Ã™â€žÃ›Å’

| ID | Ã˜Â¹Ã™â€ Ã™Ë†Ã˜Â§Ã™â€  | Priority | Status | Dependency | Branch |
|---|---|---:|---|---|---|
| GOV-001 | Ã™â€¦Ã˜Â¹Ã™â€¦Ã˜Â§Ã˜Â±Ã›Å’ Ã™Ë† Ã™â€šÃ˜Â±Ã˜Â§Ã˜Â±Ã˜Â¯Ã˜Â§Ã˜Â¯Ã™â€¡Ã˜Â§Ã›Å’ Ã˜Â¯Ã˜Â§Ã˜Â®Ã™â€žÃ›Å’ | P0 | done | - | - |
| GOV-002 | solution skeleton | P0 | done | GOV-001 | - |
| GOV-003 | Gitflow Ã™Ë† versioning | P0 | done | GOV-002 | - |
| BIND-001 | Symbol hierarchy Ã™Ë† ErrorTypeSymbol | P0 | done | LANG-003, META-001 | feature/binder/symbols |
| BIND-002 | Binder chain Ã™Ë† LookupResult Ã˜Â¨Ã˜Â§ Ã˜Â¯Ã™â€žÃ›Å’Ã™â€ž Ã˜Â´ÃšÂ©Ã˜Â³Ã˜Âª | P0 | done | BIND-001 | feature/binder/lookup |
| LANG-001 | grammar/token model Ã˜Â¨Ã˜Â±Ã˜Â§Ã›Å’ C# 6 Ã˜ÂªÃ˜Â§ 15 | P0 | ready | GOV-001 | feature/language/token-model |
| EF-005 | provenance/confidence Ã™Ë† IsComplete | P1 | ready | EF-001 | feature/ef/provenance |
| KNOW-006 | concept registry Ã˜Â¨Ã˜Â±Ã˜Â§Ã›Å’ CLR/C# | P1 | ready | GOV-001 | feature/knowledge/curriculum |

## Ã™â€¦Ã˜Â³Ã›Å’Ã˜Â± MVP Ã˜Â¯Ã™â€šÃ˜ÂªÃ¢â‚¬Å’Ã™â€¦Ã˜Â­Ã™Ë†Ã˜Â±

```text
LANG-001 Ã¢â€ â€™ LANG-002 Ã¢â€ â€™ LANG-003
                         Ã¢â€ â€œ
META-001 Ã¢â€ â€™ BIND-001 Ã¢â€ â€™ BIND-002 Ã¢â€ â€™ BIND-003 Ã¢â€ â€™ BIND-005
                                      Ã¢â€ â€œ
                               IDE-002 + QA-001
                                      Ã¢â€ â€œ
                                  EF-005
```

MVP Ã˜Â¨Ã˜Â§Ã›Å’Ã˜Â¯ Ã˜Â§Ã˜Â¨Ã˜ÂªÃ˜Â¯Ã˜Â§ subset Ã™â€¦Ã˜Â´Ã˜Â®Ã˜Âµ C# 10+ Ã™Ë† .NET 8 Ã˜Â±Ã˜Â§ Ã˜Â¨Ã˜Â§ compile-valid rate Ã˜Â¨Ã˜Â§Ã™â€žÃ˜Â§ Ã™Â¾Ã™Ë†Ã˜Â´Ã˜Â´ Ã˜Â¯Ã™â€¡Ã˜Â¯Ã˜â€º Ã˜Â³Ã™Â¾Ã˜Â³ capability matrix Ã˜Â¨Ã™â€¡ C# 6 Ã˜ÂªÃ˜Â§ 15 ÃšÂ¯Ã˜Â³Ã˜ÂªÃ˜Â±Ã˜Â´ Ã™â€¦Ã›Å’Ã¢â‚¬Å’Ã›Å’Ã˜Â§Ã˜Â¨Ã˜Â¯.

## Ã˜ÂªÃ˜Â±Ã˜ÂªÃ›Å’Ã˜Â¨ Ã˜Â§Ã˜Â¬Ã˜Â±Ã˜Â§Ã›Å’Ã›Å’ Ã˜Â¨Ã˜Â§Ã™â€šÃ›Å’Ã¢â‚¬Å’Ã™â€¦Ã˜Â§Ã™â€ Ã˜Â¯Ã™â€¡

Ã˜Â§Ã›Å’Ã™â€  Ã˜ÂµÃ™Â Ã˜ÂªÃ˜Â±Ã˜ÂªÃ›Å’Ã˜Â¨ Ã˜Â±Ã˜Â³Ã™â€¦Ã›Å’ Ã˜Â§Ã˜Â¬Ã˜Â±Ã˜Â§Ã˜Â³Ã˜ÂªÃ˜â€º Ã™â€¡Ã˜Â± Ã˜ÂªÃ˜Â³ÃšÂ© Ã˜Â¨Ã˜Â¹Ã˜Â¯ Ã˜Â§Ã˜Â² validation Ã™Ë† merge Ã˜ÂªÃ˜Â³ÃšÂ© Ã™â€šÃ˜Â¨Ã™â€žÃ›Å’ Ã˜Â´Ã˜Â±Ã™Ë†Ã˜Â¹ Ã™â€¦Ã›Å’Ã¢â‚¬Å’Ã˜Â´Ã™Ë†Ã˜Â¯.

```text
LANG-001 Ã¢â€ â€™ LANG-002 Ã¢â€ â€™ LANG-003 Ã¢â€ â€™ LANG-004
Ã¢â€ â€™ META-001 Ã¢â€ â€™ META-002 Ã¢â€ â€™ BIND-005 Ã¢â€ â€™ BIND-006 Ã¢â€ â€™ BIND-007 Ã¢â€ â€™ BIND-008
Ã¢â€ â€™ NUGET-001 Ã¢â€ â€™ NUGET-002 Ã¢â€ â€™ NUGET-003
Ã¢â€ â€™ IDE-001 Ã¢â€ â€™ IDE-002 Ã¢â€ â€™ IDE-003 Ã¢â€ â€™ IDE-004 Ã¢â€ â€™ IDE-005 Ã¢â€ â€™ IDE-006
Ã¢â€ â€™ EF-005 Ã¢â€ â€™ EF-006 Ã¢â€ â€™ EF-007 Ã¢â€ â€™ EF-008 Ã¢â€ â€™ EF-009 Ã¢â€ â€™ EF-010
Ã¢â€ â€™ KNOW-006 Ã¢â€ â€™ KNOW-002 Ã¢â€ â€™ KNOW-003 Ã¢â€ â€™ KNOW-004 Ã¢â€ â€™ KNOW-005
Ã¢â€ â€™ QA-001 Ã¢â€ â€™ QA-002 Ã¢â€ â€™ QA-003 Ã¢â€ â€™ QA-004 Ã¢â€ â€™ REL-001 Ã¢â€ â€™ REL-002
```

Ã˜Â´Ã˜Â±Ã˜Â·Ã¢â‚¬Å’Ã™â€¡Ã˜Â§Ã›Å’ Ã˜Â®Ã˜Â±Ã™Ë†Ã˜Â¬ Ã™â€¡Ã˜Â± Ã™â€¦Ã˜Â±Ã˜Â­Ã™â€žÃ™â€¡ Ã˜Â¯Ã˜Â± Ã˜Â¬Ã˜Â¯Ã™Ë†Ã™â€ž backlog Ã™Ë† Definition of Done Ã˜Â«Ã˜Â¨Ã˜Âª Ã˜Â´Ã˜Â¯Ã™â€¡Ã¢â‚¬Å’Ã˜Â§Ã™â€ Ã˜Â¯Ã˜â€º task Ã˜Â¨Ã˜Â¹Ã˜Â¯Ã›Å’ Ã™ÂÃ™â€šÃ˜Â· Ã™Â¾Ã˜Â³ Ã˜Â§Ã˜Â² Ã™â€¦Ã™Ë†Ã™ÂÃ™â€šÃ›Å’Ã˜Âª buildÃ˜Å’ smoke/regression testÃ˜Å’ Graphify update Ã™Ë† merge Ã™â€¦Ã˜Â­Ã™â€žÃ›Å’ Ã™ÂÃ˜Â¹Ã˜Â§Ã™â€ž Ã™â€¦Ã›Å’Ã¢â‚¬Å’Ã˜Â´Ã™Ë†Ã˜Â¯.

## Backlog Ã˜ÂªÃ™ÂÃ˜ÂµÃ›Å’Ã™â€žÃ›Å’

### Compiler core

| ID | Ã˜Â®Ã˜Â±Ã™Ë†Ã˜Â¬Ã›Å’ | Acceptance criteria | Validation |
|---|---|---|---|
| LANG-001 | feature matrix Ã™Ë† token specification | Ã™â€¡Ã˜Â± feature Ã›Å’ÃšÂ© language gate Ã™Ë† fixture Ã˜Â¯Ã˜Â§Ã˜Â±Ã˜Â¯ | specification review |
| LANG-002 | lexer lossless Ã™Ë† incremental | round-trip trivia Ã™Ë† recovery Ã˜Â¨Ã˜Â¯Ã™Ë†Ã™â€  crash | unit + fuzz |
| LANG-003 | parser Ã™Ë† syntax tree | parse/print Ã™â€¦Ã˜ÂªÃ™â€  corpus Ã˜Â±Ã˜Â§ Ã˜Â­Ã™ÂÃ˜Â¸ ÃšÂ©Ã™â€ Ã˜Â¯ | golden corpus |
| BIND-001 | symbol hierarchy Ã™Ë† error symbols | source/metadata symbol Ã™â€¦Ã˜Â´Ã˜ÂªÃ˜Â±ÃšÂ© Ã™Ë† candidate Ã˜Â­Ã™ÂÃ˜Â¸ Ã˜Â´Ã™Ë†Ã˜Â¯ | symbol dump |
| BIND-002 | binder chain Ã™Ë† lookup result | NotFound/Inaccessible/WrongArity/Ambiguous Ã˜ÂªÃ™ÂÃšÂ©Ã›Å’ÃšÂ© Ã˜Â´Ã™Ë†Ã˜Â¯ | conformance |
| BIND-003 | conversion classifier | kind Ã˜Â³Ã˜Â§Ã˜Â®Ã˜ÂªÃ˜Â§Ã˜Â±Ã›Å’Ã˜Â§Ã™ÂÃ˜ÂªÃ™â€¡ Ã™Ë† cache version-aware Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯ | conversion corpus |
| BIND-005 | overload resolution | candidate rejection reason Ã™Ë† best member Ã˜Â¯Ã˜Â±Ã˜Â³Ã˜Âª Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯ | spec + differential CI |
| BIND-006 | generic inference Ã™Ë† LINQ | Ã˜Â²Ã™â€ Ã˜Â¬Ã›Å’Ã˜Â±Ã™â€¡Ã¢â‚¬Å’Ã™â€¡Ã˜Â§Ã›Å’ Select/Where Ã™â€ Ã™Ë†Ã˜Â¹ Ã˜Â¯Ã˜Â±Ã˜Â³Ã˜Âª Ã˜Â¯Ã˜Â§Ã˜Â´Ã˜ÂªÃ™â€¡ Ã˜Â¨Ã˜Â§Ã˜Â´Ã™â€ Ã˜Â¯ | LINQ corpus |
| BIND-007 | speculative verification | diagnostic Ã˜Â¬Ã˜Â¯Ã›Å’Ã˜Â¯ Ã™â€ Ã˜Â³Ã˜Â¨Ã˜Âª Ã˜Â¨Ã™â€¡ baseline Ã˜ÂªÃ˜Â´Ã˜Â®Ã›Å’Ã˜Âµ Ã˜Â¯Ã˜Â§Ã˜Â¯Ã™â€¡ Ã˜Â´Ã™Ë†Ã˜Â¯ | compile-valid benchmark |

### Metadata Ã™Ë† project index

| ID | Ã˜Â®Ã˜Â±Ã™Ë†Ã˜Â¬Ã›Å’ | Acceptance criteria | Validation |
|---|---|---|---|
| META-001 | metadata reader Ã˜Â¯Ã˜Â§Ã˜Â®Ã™â€žÃ›Å’ Ã˜Â¨Ã˜Â§ BCL | reference assembly Ã™Ë† XML docs Ã˜Â§Ã›Å’Ã™â€ Ã˜Â¯ÃšÂ©Ã˜Â³ Ã˜Â´Ã™Ë†Ã™â€ Ã˜Â¯ | assembly corpus |
| NUGET-001 | assets/project watcher | PackageReference Ã™Ë† TFM Ã˜ÂªÃ˜ÂºÃ›Å’Ã›Å’Ã˜Â± incremental Ã˜Â¨Ã˜Â¯Ã™â€¡Ã˜Â¯ | fixture restore |
| NUGET-002 | package API index | direct/transitive compile assets Ã˜Â¯Ã˜Â±Ã˜Â³Ã˜Âª Ã˜Â¨Ã˜Â§Ã˜Â´Ã™â€ Ã˜Â¯ | assets comparison |
| NUGET-003 | cache content-addressed | key Ã˜Â´Ã˜Â§Ã™â€¦Ã™â€ž id/version/tfm/hash Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯ | cache tests |

### EF Ã™Ë† rule knowledge

| ID | Ã˜Â®Ã˜Â±Ã™Ë†Ã˜Â¬Ã›Å’ | Acceptance criteria | Validation |
|---|---|---|---|
| EF-005 | provenance Ã™Ë† IsComplete | Ã™â€¦Ã˜Â¯Ã™â€ž Ã™â€ Ã˜Â§Ã™â€šÃ˜Âµ Ã™â€¡Ã›Å’Ãšâ€  negative rule Ã™â€šÃ˜Â·Ã˜Â¹Ã›Å’ Ã˜ÂªÃ™Ë†Ã™â€žÃ›Å’Ã˜Â¯ Ã™â€ ÃšÂ©Ã™â€ Ã˜Â¯ | model corpus |
| EF-006 | conventions/annotations | key/FK/navigation Ã˜Â¨Ã˜Â§ Ã™â€¦Ã˜Â¯Ã™â€ž Ã™Ë†Ã˜Â§Ã™â€šÃ˜Â¹Ã›Å’ Ã™â€¦Ã™â€ Ã˜Â·Ã˜Â¨Ã™â€š Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯ | ToDebugString oracle |
| EF-007 | Fluent interpreter | symbol identity Ã™Ë† lambda property resolve Ã˜Â´Ã™Ë†Ã˜Â¯ | EF fixture |
| EF-009 | translation matrix | verdict Ã˜Â¨Ã˜Â± Ã˜Â§Ã˜Â³Ã˜Â§Ã˜Â³ provider/version Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯ | ToQueryString CI |
| EF-010 | query completion | Include/ThenInclude Ã™Ë† IQueryable context Ã™â€¦Ã˜Â¹Ã˜ÂªÃ˜Â¨Ã˜Â± Ã˜Â¨Ã˜Â§Ã˜Â´Ã™â€ Ã˜Â¯ | compile-valid rate |
| KNOW-002 | official crawler | RSS/sitemapÃ˜Å’ rate limit Ã™Ë† canonical URL Ã˜Â«Ã˜Â¨Ã˜Âª Ã˜Â´Ã™Ë†Ã˜Â¯ | crawler fixtures |
| KNOW-004 | signed rule pack | hash/signatureÃ˜Å’ compatibility Ã™Ë† rollback ÃšÂ©Ã˜Â§Ã˜Â± ÃšÂ©Ã™â€ Ã˜Â¯ | package tests |

### Editor Ã™Ë† ÃšÂ©Ã›Å’Ã™ÂÃ›Å’Ã˜Âª

| ID | Ã˜Â®Ã˜Â±Ã™Ë†Ã˜Â¬Ã›Å’ | Acceptance criteria | Validation |
|---|---|---|---|
| IDE-001 | transport Ã˜Â§Ã˜Â®Ã˜ÂªÃ˜ÂµÃ˜Â§Ã˜ÂµÃ›Å’ | request cancellation Ã™Ë† version guard Ã˜Â¯Ã˜Â§Ã˜Â´Ã˜ÂªÃ™â€¡ Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯ | protocol integration |
| IDE-002 | context classifier | expected typeÃ˜Å’ lambdaÃ˜Å’ LINQ Ã™Ë† async Ã˜ÂªÃ˜Â´Ã˜Â®Ã›Å’Ã˜Âµ Ã˜Â¯Ã˜Â§Ã˜Â¯Ã™â€¡ Ã˜Â´Ã™Ë†Ã™â€ Ã˜Â¯ | context corpus |
| IDE-005 | explainable ranker | score featureÃ™â€¡Ã˜Â§ Ã™â€šÃ˜Â§Ã˜Â¨Ã™â€ž Ã™â€¦Ã˜Â´Ã˜Â§Ã™â€¡Ã˜Â¯Ã™â€¡ Ã˜Â¨Ã˜Â§Ã˜Â´Ã™â€ Ã˜Â¯ | ranking tests |
| QA-001 | corpus Ã™Ë†Ã˜Â§Ã™â€šÃ˜Â¹Ã›Å’ | Ã™â€¦Ã˜Â¬Ã™Ë†Ã˜Â²Ã˜Å’ Ã™â€ Ã˜Â³Ã˜Â®Ã™â€¡ Ã˜Â²Ã˜Â¨Ã˜Â§Ã™â€  Ã™Ë† TFM Ã˜Â«Ã˜Â¨Ã˜Âª Ã˜Â´Ã˜Â¯Ã™â€¡ Ã˜Â¨Ã˜Â§Ã˜Â´Ã˜Â¯ | corpus manifest |
| QA-002 | metric harness | Top-kÃ˜Å’ MRRÃ˜Å’ compile-valid Ã™Ë† latency Ã˜Â«Ã˜Â¨Ã˜Âª Ã˜Â´Ã™Ë†Ã™â€ Ã˜Â¯ | benchmark run |
| REL-002 | alpha release | buildÃ˜Å’ packÃ˜Å’ changelog Ã™Ë† tag immutable Ã˜Â¨Ã˜Â§Ã˜Â´Ã™â€ Ã˜Â¯ | release checklist |

## Definition of Ready

- contract Ã™Ë† dependency Ã™â€¦Ã˜Â´Ã˜Â®Ã˜Âµ Ã˜Â§Ã˜Â³Ã˜Âª.
- test fixture Ã›Å’Ã˜Â§ Ã˜Â±Ã™Ë†Ã˜Â´ Ã˜Â³Ã˜Â§Ã˜Â®Ã˜Âª fixture Ã™â€¦Ã˜Â´Ã˜Â®Ã˜Âµ Ã˜Â§Ã˜Â³Ã˜Âª.
- branch name Ã™Ë† Ã™â€ Ã™Ë†Ã˜Â¹ commit Ã˜ÂªÃ˜Â¹Ã›Å’Ã›Å’Ã™â€  Ã˜Â´Ã˜Â¯Ã™â€¡ Ã˜Â§Ã˜Â³Ã˜Âª.
- Ã™â€¦Ã˜Â¹Ã™â€žÃ™Ë†Ã™â€¦ Ã˜Â§Ã˜Â³Ã˜Âª ÃšÂ©Ã™â€¡ Ã˜ÂªÃ˜ÂºÃ›Å’Ã›Å’Ã˜Â± external dependency Ã™Ë†Ã˜Â§Ã˜Â±Ã˜Â¯ runtime Ã™â€¦Ã›Å’Ã¢â‚¬Å’ÃšÂ©Ã™â€ Ã˜Â¯ Ã›Å’Ã˜Â§ Ã™â€ Ã™â€¡.

## Definition of Done

- acceptance criteria ÃšÂ©Ã˜Â§Ã™â€¦Ã™â€ž Ã˜Â§Ã˜Â³Ã˜Âª.
- build Ã™Ë† test scope Ã˜Â³Ã˜Â¨Ã˜Â² Ã˜Â§Ã˜Â³Ã˜Âª.
- Graphify update Ã˜Â§Ã™â€ Ã˜Â¬Ã˜Â§Ã™â€¦ Ã˜Â´Ã˜Â¯Ã™â€¡ Ã˜Â§Ã˜Â³Ã˜Âª.
- docs/task state Ã˜Â¨Ã™â€¡Ã¢â‚¬Å’Ã˜Â±Ã™Ë†Ã˜Â² Ã˜Â§Ã˜Â³Ã˜Âª.
- commit Conventional Ã™Ë† branch Ã™â€šÃ˜Â§Ã˜Â¨Ã™â€ž review Ã˜Â§Ã˜Â³Ã˜Âª.
- risk Ã™Ë† shortcut Ã˜Â§Ã˜Â­Ã˜ÂªÃ™â€¦Ã˜Â§Ã™â€žÃ›Å’ Ã˜Â«Ã˜Â¨Ã˜Âª Ã˜Â´Ã˜Â¯Ã™â€¡ Ã˜Â§Ã˜Â³Ã˜Âª.

