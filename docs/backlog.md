# Product Backlog — Suggestion Accuracy

The previous implementation backlog is complete. This backlog contains the next work for accurate, context-aware suggestions.

## Priority order

| Order | ID | Priority | Status | Dependency | Branch |
|---:|---|---:|---|---|---|
| 1 | SUG-001 | P0 | ready | existing LSP completion | `feature/suggestion/project-context` |
| 2 | SUG-002 | P0 | ready | SUG-001 | `feature/suggestion/nuget-api` |
| 3 | SUG-003 | P0 | ready | SUG-001 | `feature/suggestion/assembly-evidence` |
| 4 | SUG-004 | P0 | ready | SUG-001 | `feature/suggestion/csharp-semantic-context` |
| 5 | SUG-005 | P1 | ready | SUG-002,SUG-003,SUG-004 | `feature/suggestion/runtime-knowledge` |
| 6 | SUG-006 | P1 | ready | SUG-005 | `feature/suggestion/evidence-ranking` |
| 7 | SUG-007 | P1 | ready | SUG-006 | `feature/suggestion/incremental-cache` |
| 8 | SUG-008 | P1 | ready | SUG-006 | `feature/suggestion/accuracy-corpus` |

## Remaining tasks

### SUG-001 — Project context snapshot

Build an immutable per-document context containing language version, target framework, project configuration, active imports, project references, and loaded document version.

Acceptance criteria: completion receives the correct snapshot for the current document, stale snapshots are rejected, and unknown context lowers confidence instead of producing certain suggestions.

Validation: multi-project and multi-TFM context tests.

### SUG-002 — NuGet API suggestions

Feed direct and transitive NuGet package APIs into completion using the existing project/assets index and metadata cache.

Acceptance criteria: package and version determine available namespaces/types/members; incompatible TFM assets are excluded; package metadata is read-only and local.

Validation: package fixture with positive, wrong-version, and missing-asset cases.

### SUG-003 — Loaded assembly evidence

Index project output and referenced assemblies, including type forwarders and target framework metadata, then use that evidence to gate suggestions.

Acceptance criteria: suggestions only use assemblies visible to the active project/TFM and preserve assembly identity in metadata.

Validation: assembly reference and type-forwarder fixture.

### SUG-004 — C# semantic context

Replace identifier scanning with the internal lexer/parser/binder context for namespace, type, member, invocation, generic, and argument positions. Roslyn remains a differential CI oracle, not a runtime authority.

Acceptance criteria: completion candidates are scoped to the receiver/type and overload context; incomplete code keeps valid candidates without guessing unknown types.

Validation: C# corpus for member, invocation, generic, lambda, LINQ, and incomplete-edit cases.

### SUG-005 — CLR, DLR, and CIS knowledge evidence

Connect CLR/C# concept packs to rules for runtime types, dynamic dispatch, reflection, async, allocation, and language-service explanations. CIS means the project’s C# language-intelligence service boundary and must remain local/read-only.

Acceptance criteria: every analytical suggestion has concept/rule/source references and a clear evidence requirement; no rule runs from crawled text.

Validation: knowledge-to-suggestion integration tests and signed-pack checks.

### SUG-006 — Evidence-based ranking

Rank candidates using prefix, semantic context, package/assembly evidence, language/TFM compatibility, confidence, and user settings. Deduplicate by symbol, edit range, and intent.

Acceptance criteria: unsupported or weakly evidenced candidates cannot outrank valid local candidates; LSP metadata reports rule, confidence, and source.

Validation: ranking fixtures with precision, false-positive, and ambiguity assertions.

### SUG-007 — Incremental context and cache

Cache project/package/assembly indexes by content hash and invalidate them on document, project, assets, or configuration changes.

Acceptance criteria: completion stays responsive under repeated edits and never returns a stale version.

Validation: cache-hit, invalidation, cancellation, and p95 latency tests.

### SUG-008 — Suggestion accuracy corpus

Create a regression corpus covering the supported C# versions, TFMs, packages, assemblies, CLR/DLR concepts, and negative cases.

Acceptance criteria: every new rule has positive and negative cases, with precision, recall, compile-valid rate, and latency recorded.

Validation: repeatable corpus benchmark in CI.
