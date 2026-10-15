# Product Backlog — Remaining Work

Completed items are archived in `docs/workflow/state.json`. This file contains only the work that remains, ordered by the dependency that matters most to users.

## Priority order

| Order | ID | Priority | Status | Dependency | Branch |
|---:|---|---:|---|---|---|
| 1 | BIND-006 | P1 | ready | BIND-005 | `feature/binder/generic-inference` |
| 2 | BIND-007 | P1 | ready | BIND-006 | `feature/binder/speculative-verification` |

## Remaining tasks

### BIND-006 — Generic inference and LINQ

Infer type arguments for the supported `Select` and `Where` chains.

Acceptance criteria: inferred element and result types are correct for the LINQ corpus.

Validation: compile-valid LINQ corpus.

### BIND-007 — Speculative verification

Compare the edited document with its baseline and report only new diagnostics.

Acceptance criteria: new diagnostics are separated from baseline diagnostics without suppressing existing errors.

Validation: compile-valid benchmark.
