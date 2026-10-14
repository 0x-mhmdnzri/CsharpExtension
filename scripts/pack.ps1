#!/usr/bin/env pwsh
# Builds Release and packs library projects into artifacts/packages.
$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

$out = Join-Path $root 'artifacts/packages'
New-Item -ItemType Directory -Force -Path $out | Out-Null

dotnet build (Join-Path $root 'DotnetExpertAdvisor.slnx') -c Release
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$projects = @(
    'src/Shared/Shared.csproj'
    'src/Protocol/Protocol.csproj'
    'src/RoslynHost/RoslynHost.csproj'
    'src/SemanticGraph/SemanticGraph.csproj'
    'src/RuleEngine/RuleEngine.csproj'
    'src/RankingEngine/RankingEngine.csproj'
    'src/KnowledgeEngine/KnowledgeEngine.csproj'
    'src/RuntimeModel/RuntimeModel.csproj'
)

foreach ($proj in $projects) {
    $path = Join-Path $root $proj
    if (-not (Test-Path $path)) { Write-Warning "Skip missing $proj"; continue }
    dotnet pack $path -c Release --no-build -o $out
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}

Write-Host "Packages written to $out"
Get-ChildItem $out -Filter *.nupkg | ForEach-Object { $_.Name }
