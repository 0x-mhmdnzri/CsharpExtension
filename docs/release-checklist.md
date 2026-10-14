# Release checklist — alpha

Use this checklist for every pre-1.0 tagged release. Tags are **immutable**.

## Before cut

- [ ] `docs/workflow/state.json` points at the release task and branch
- [ ] `Directory.Build.props` `VersionPrefix` / `VersionSuffix` match the intended tag
- [ ] `CHANGELOG.md` has a section for this version
- [ ] Smoke suite passes: `dotnet run --project tests/Architecture.Tests`
- [ ] `dotnet build DotnetExpertAdvisor.slnx -c Release` succeeds
- [ ] No open feature branches required for this alpha

## Pack

```powershell
./scripts/pack.ps1
```

- [ ] `artifacts/packages/` contains `.nupkg` files with version `0.1.0-alpha.1`
- [ ] Package ids use the `DotnetExpertAdvisor.*` prefix

## Tag and publish

```powershell
git switch main
git merge --no-ff release/0.1.0-alpha.1 -m "release: 0.1.0-alpha.1"
git tag -a v0.1.0-alpha.1 -m "Release 0.1.0-alpha.1"
git push origin main
git push origin v0.1.0-alpha.1
git switch develop
git merge --no-ff release/0.1.0-alpha.1 -m "merge: release 0.1.0-alpha.1 back to develop"
git push origin develop
```

- [ ] Annotated tag `v0.1.0-alpha.1` exists and matches `PackageVersion`
- [ ] GitHub Release notes reference `CHANGELOG.md`
- [ ] Do **not** move or retag; ship a new patch/pre-release instead

## After

- [ ] Delete local/remote `release/0.1.0-alpha.1` after both merges
- [ ] Advance workflow state to the next backlog item
