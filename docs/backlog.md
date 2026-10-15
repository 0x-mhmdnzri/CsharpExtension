# Product Backlog — Remaining Work

Completed items are archived in `docs/workflow/state.json`. This file contains only the work that remains, ordered by the dependency that matters most to users.

## Priority order

| Order | ID | Priority | Status | Dependency | Branch |
|---:|---|---:|---|---|---|
| 1 | BIND-007 | P1 | ready | BIND-006 | `feature/binder/speculative-verification` |

## Remaining tasks

### BIND-007 — Speculative verification

Compare the edited document with its baseline and report only new diagnostics.

Acceptance criteria: new diagnostics are separated from baseline diagnostics without suppressing existing errors.

Validation: compile-valid benchmark.
