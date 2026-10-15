# Product Backlog — Remaining Work

Completed items are archived in `docs/workflow/state.json`. This file contains only the work that remains, ordered by the dependency that matters most to users.

## Priority order

| Order | ID | Priority | Status | Dependency | Branch |
|---:|---|---:|---|---|---|
| 1 | KNOW-006 | P1 | ready | GOV-001 | `feature/knowledge/curriculum` |
| 2 | BIND-005 | P1 | ready | BIND-004 | `feature/binder/overload-resolution` |
| 3 | BIND-006 | P1 | ready | BIND-005 | `feature/binder/generic-inference` |
| 4 | BIND-007 | P1 | ready | BIND-006 | `feature/binder/speculative-verification` |

## Remaining tasks

### KNOW-006 — CLR/C# concept registry

Add the remaining CLR and C# concepts needed by completion explanations and ranking.

Acceptance criteria: concepts are addressable by stable IDs, source references are retained, and unknown concepts remain explicit.

Validation: registry unit tests and lookup smoke test.

### BIND-005 — Overload resolution

Return the best member and candidate rejection reasons for overloaded calls.

Acceptance criteria: overload choice and rejection reason are correct for the supported C# corpus.

Validation: specification tests and differential CI.

### BIND-006 — Generic inference and LINQ

Infer type arguments for the supported `Select` and `Where` chains.

Acceptance criteria: inferred element and result types are correct for the LINQ corpus.

Validation: compile-valid LINQ corpus.

### BIND-007 — Speculative verification

Compare the edited document with its baseline and report only new diagnostics.

Acceptance criteria: new diagnostics are separated from baseline diagnostics without suppressing existing errors.

Validation: compile-valid benchmark.
