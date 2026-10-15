# ADR-009: Roslyn is the semantic authority for valid completion candidates

## Decision

Roslyn parses the document, builds the compilation and semantic model, and supplies the valid completion candidate set. Internal ranking and project-specific signals may reorder or filter that set, but they must not invent symbols outside it.

## Consequences

- The language server depends on `Microsoft.CodeAnalysis.CSharp`.
- Candidate correctness comes from Roslyn; intent ranking remains a separate concern.
- Missing project references reduce recall until workspace/project loading is implemented.
- A replay harness and ranking signals are required to improve ordering without weakening semantic validity.
