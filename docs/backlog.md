# Product Backlog — High-Accuracy Suggestions

This backlog turns the product direction into executable work. Correct candidates come from Roslyn; the internal engine ranks intent signals and never invents symbols.

## Priority order

| Order | ID | Priority | Status | Dependency | Branch |
|---:|---|---:|---|---|---|
| 1 | ARCH-001 | P0 | done | existing RoslynHost skeleton | `feature/roslyn/semantic-authority` |
| 2 | SUG-001 | P0 | done | ARCH-001 | `feature/suggestion/project-context` |
| 3 | SUG-009 | P0 | done | ARCH-001,SUG-001 | `feature/suggestion/roslyn-candidates` |
| 4 | SUG-010 | P0 | done | SUG-009 | `feature/suggestion/expected-type-filter` |
| 5 | QA-003 | P0 | done | SUG-009 | `feature/qa/suggestion-replay` |
| 6 | SUG-011 | P1 | done | SUG-010,QA-003 | `feature/suggestion/intent-signals` |
| 7 | SUG-002 | P1 | done | SUG-001,SUG-009 | `feature/suggestion/nuget-api` |
| 8 | SUG-003 | P1 | done | SUG-001,SUG-009 | `feature/suggestion/assembly-evidence` |
| 9 | SUG-012 | P1 | done | SUG-002,SUG-003,QA-003 | `feature/suggestion/project-patterns` |
| 10 | SUG-013 | P1 | done | SUG-011,QA-003 | `feature/suggestion/confidence-calibration` |
| 11 | IDX-001 | P0 | done | SUG-003 | `feature/index/workspace-inventory` |
| 12 | IDX-002 | P0 | done | IDX-001 | `feature/index/roslyn-multi-document` |
| 13 | IDX-003 | P0 | done | IDX-002 | `feature/index/incremental-cache` |
| 14 | IDX-004 | P1 | done | IDX-002 | `feature/index/dependency-graph` |
| 15 | SUG-004 | P1 | ready | IDX-002 | `feature/suggestion/csharp-semantic-context` |
| 16 | SUG-005 | P1 | ready | SUG-002,SUG-003,SUG-004 | `feature/suggestion/runtime-knowledge` |
| 17 | SUG-006 | P1 | ready | SUG-005,SUG-013 | `feature/suggestion/evidence-ranking` |
| 18 | SUG-008 | P1 | ready | QA-003,SUG-013 | `feature/suggestion/accuracy-corpus` |
| 19 | SUG-014 | P2 | ready | SUG-008,SUG-013 | `feature/suggestion/ml-reranker` |
| 20 | SUG-015 | P2 | ready | SUG-013,SUG-014 | `feature/suggestion/mru-personalization` |
| 21 | SUG-016 | P3 | deferred | SUG-009,SUG-013 | `feature/suggestion/optional-llm-signal` |

## Remaining tasks

### ARCH-001 — Roslyn semantic authority (done)

Make Roslyn the source of valid syntax, symbols, types, expected types, overloads, and semantic model data. Keep the internal engine as the intent/ranking layer and use Roslyn differential checks where product independence requires it.

Acceptance criteria: runtime completion candidates are Roslyn-backed, unsupported candidates are never invented, and the architecture decision is explicit about the Roslyn runtime dependency.

Validation: Roslyn-backed language-server smoke test and architecture fixture with incomplete C# edits.

### SUG-001 — Project context snapshot (done)

Capture language version, target framework, project configuration, imports, references, document version, and active project for every completion request.

Acceptance criteria: stale or unknown context lowers confidence and cannot produce a certain suggestion.

Validation: multi-project and multi-TFM fixtures, including ambiguous-context confidence handling.

### SUG-009 — Roslyn candidate normalization (done)

Normalize Roslyn completion candidates into the project LSP model with symbol identity, display text, edit range, kind, overload data, and documentation.

Acceptance criteria: duplicate candidates collapse by symbol/edit/intent and every item retains its originating symbol identity.

Validation: candidate identity, overload, duplicate, documentation, kind, and edit-range fixtures.

### SUG-010 — Expected-type hard filtering (done)

Infer the expected type at the cursor and remove or demote candidates that cannot satisfy it, including argument, assignment, return, initializer, lambda, generic, and overload contexts.

Acceptance criteria: invalid candidates never outrank valid candidates; unknown expected type keeps candidates as uncertain rather than deleting them.

Validation: assignment and invocation argument positive/negative type-constraint fixtures.

### QA-003 — Suggestion replay harness (done)

Build an offline replay harness that removes real identifiers and measures the original symbol rank after 0, 1, 2, and 3 typed characters.

Acceptance criteria: Top-1 accuracy, Top-5 accuracy, keystrokes saved, compile-valid rate, latency, and false-positive rate are recorded per feature.

Validation: repeatable four-prefix replay fixture with rank, accuracy, keystroke, compile, false-positive, and latency metrics.

### SUG-011 — Cheap intent signals (done)

Add locality, recently declared unused locals/parameters, object initializer gaps, missing enum cases, missing interface members, name token similarity, and cursor history signals.

Acceptance criteria: each signal is independently measurable in replay and cannot override hard type constraints.

Validation: Roslyn intent trace and unused-local ranking fixture; hard type constraints remain authoritative.

### SUG-002 — NuGet API suggestions (done)

Use direct/transitive package references, versions, TFM assets, and package metadata to scope API candidates.

Acceptance criteria: incompatible or unavailable package APIs are excluded and package evidence is shown in item metadata.

Validation: package assembly positive fixture and missing-asset/context fallback; package metadata is summarized in LSP data.

### SUG-003 — Loaded assembly evidence (done)

Use project outputs, referenced assemblies, type forwarders, and target framework metadata to gate candidates.

Acceptance criteria: suggestions only use assemblies visible to the active project/TFM and preserve assembly identity.

Validation: MSBuild reference-path loading, package assembly identity, and type-forwarder evidence capture.

### SUG-012 — Project pattern signals (done)

Learn deterministic co-occurrence, receiver member frequency, call order, and team naming conventions from the local project.

Acceptance criteria: local patterns improve replay metrics without overriding type or assembly validity; no source leaves the device.

Validation: deterministic project frequency/call-order index and completion ranking trace.

### SUG-013 — Confidence calibration and preselect (done)

Calibrate confidence and preselect only high-confidence items. Use conservative match/locality ordering when confidence is low.

Acceptance criteria: low-confidence items are never preselected and calibration is measured against Top-1 correctness.

Validation: conservative calibration thresholds and preselect regression fixtures.

### IDX-001 — Workspace inventory (done)

Scan every project document, project file, package asset, generated output, and loaded assembly into a versioned workspace inventory. Exclude bin/obj/generated noise unless Roslyn marks it as a reference.

Acceptance criteria: every source document and project/TFM/package/assembly input has a stable identity, timestamp/hash, and scan status; inaccessible files are reported.

Validation: source/project hash inventory with build-output exclusion and inaccessible-file status.

### IDX-002 — Roslyn multi-document compilation (done)

Build Roslyn compilations from the indexed documents and project references so completion can resolve symbols declared in other files and projects.

Acceptance criteria: cross-file and cross-project symbols resolve only through valid project references and diagnostics remain cancellation-aware.

Validation: cross-file source fixture with positive symbol resolution and cancellation-aware parsing.

### IDX-003 — Incremental workspace cache (done)

Cache document syntax, semantic, package, and assembly indexes by content hash and invalidate only affected projects and dependents.

Acceptance criteria: unchanged files are not reparsed; edits never expose stale symbols.

Validation: unchanged cross-file syntax-tree reuse fixture.

### IDX-004 — Dependency graph (done)

Index project references, package dependencies, assembly references, type forwarders, and symbol relationships for ranking evidence.

Acceptance criteria: every suggestion can identify the project/assembly/package path that made it visible.

Validation: project-reference and package-reference graph fixture.

### SUG-004 — C# semantic context

Use semantic context for namespace, type, member, invocation, generic, lambda, and argument positions instead of identifier scanning.

Acceptance criteria: candidates are scoped to receiver and invocation context while incomplete code remains usable without guesses.

Validation: C# semantic corpus.

### SUG-005 — CLR, DLR, and CIS knowledge evidence

Connect CLR, DLR, and the local C# language-intelligence service (CIS) concepts to evidence-backed explanations and suggestion rules.

Acceptance criteria: analytical suggestions retain concept, rule, source, confidence, and limitations; crawled text is never executable.

Validation: signed knowledge-pack integration tests.

### SUG-006 — Evidence-based ranking

Combine prefix, expected type, locality, package/assembly evidence, language/TFM compatibility, confidence, and user settings, with symbol/edit/intent deduplication.

Acceptance criteria: valid local candidates outrank weakly evidenced candidates and every ranking decision is explainable.

Validation: ranking fixtures and replay benchmark.

### SUG-007 — Incremental context and cache

Cache project, package, assembly, semantic, and pattern indexes by content hash and invalidate them on relevant changes.

Acceptance criteria: no stale result survives a version change and completion meets the measured latency target.

Validation: cache invalidation, cancellation, and p95 latency tests.

### SUG-008 — Accuracy corpus

Maintain positive and negative cases across C# versions, TFMs, packages, assemblies, CLR/DLR concepts, and incomplete edits.

Acceptance criteria: every feature has replay coverage and cannot merge when its measured precision regresses.

Validation: repeatable CI benchmark.

### SUG-014 — ML reranker

Train a small explainable reranker over the top 30–50 valid candidates using manually defined features. Run it locally through ONNX only after replay proves the feature baseline.

Acceptance criteria: improves Top-1 or keystrokes saved without reducing compile-valid rate or violating hard filters.

Validation: holdout replay and latency budget.

### SUG-015 — MRU personalization

Add per-receiver MRU with time decay and explicit handling for accepted, ignored, and rejected items.

Acceptance criteria: personalization is local, opt-out, bounded, and cannot override validity or confidence gates.

Validation: deterministic decay and privacy tests.

### SUG-016 — Optional LLM signal

Only if needed, add an opt-in LLM signal for multi-line or intent explanations. It must never replace Roslyn candidates or decide symbol validity.

Acceptance criteria: disabled by default, no code leaves the device without consent, and latency/failure fallback keeps normal completion intact.

Validation: offline-only integration tests and explicit opt-in checks.
