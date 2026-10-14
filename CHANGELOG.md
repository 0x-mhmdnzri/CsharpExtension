# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.1.0-alpha.1] - 2026-10-14

First alpha snapshot. Not production-ready; APIs and schemas may change without notice.

### Added

- Governance, ADR set, backlog, and Gitflow workflow state
- Custom language pipeline foundations: lexer, parser, recovery, token model
- Binder: symbols, lookup, member lookup, conversions
- Metadata reader, framework index, references, signatures, XML docs
- NuGet project and API indexing
- IDE navigation, completion context, diagnostics, editor features
- EF model / LINQ / performance analyzers and provider registry
- Knowledge crawler, extractor, registry, signed packs, update client
- QA corpus manifest, precision/recall/latency metrics, regression runner, quality gate
- Language server stdio bootstrap and protocol models

### Known limits

- No full Roslyn workspace host yet (custom engine path is intentional per ADR-001 / ADR-006)
- Alpha quality gates are unit-level; corpus-driven gates are not fully wired
- VSIX packaging is out of scope for this tag

[0.1.0-alpha.1]: https://github.com/0x-mhmdnzri/CsharpExtension/releases/tag/v0.1.0-alpha.1
