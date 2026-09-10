---
title: "dotnet-testing"
description: ".NET testing toolkit: the /add-coverage pipeline, CRAP analysis, mutation testing, xUnit conventions, and Aspire integration testing"
summary: "Three commands, four agents, and five skills built around finding and closing the riskiest coverage gap"
date: 2026-08-13
weight: 40
---

The .NET testing toolkit, built around a single command that finds the highest-risk untested
method, writes a mutation-validated xUnit test, reviews it for quality, commits, and opens a PR.

```bash
claude plugin install dotnet-testing@nimblepros
```

This plugin is deliberately .NET-specific — it depends on `dotnet test`, ReportGenerator,
Stryker.NET, xUnit, Shouldly, and NSubstitute. For Playwright end-to-end testing practice, see the
[playwright-testing]({{< ref "/playwright-testing" >}}) plugin.

## Commands

| Command | One-line summary |
|---|---|
| [`/dotnet-testing:add-coverage`]({{< ref "add-coverage" >}}) | Six-phase pipeline: find the highest-CRAP gap → write test → validate it has teeth → review → commit → open PR |
| [`/dotnet-testing:crap-analysis`]({{< ref "crap-analysis" >}}) | Collect coverage, generate a CRAP score report, surface the top risk hotspots |
| [`/dotnet-testing:mutation-test`]({{< ref "stryker-dotnet" >}}) | Run Stryker.NET against a source file or class, report killed vs. survived mutations |

## Skills

| Skill | One-line summary |
|---|---|
| [crap-analysis]({{< ref "crap-analysis" >}}) | CRAP scoring: complexity² × untested-ness, via OpenCover + ReportGenerator Risk Hotspots |
| [test-authoring]({{< ref "test-authoring" >}}) | xUnit house conventions: naming, Shouldly, NSubstitute, fixtures, meaningful-vs-ornamental tests |
| [test-type-selection]({{< ref "test-type-selection" >}}) | The unit → integration → e2e ladder: cheapest type that can observe the behaviour |
| [stryker-dotnet]({{< ref "stryker-dotnet" >}}) | Scoped Stryker.NET runs and killed/survived interpretation |
| [aspire-integration-testing]({{< ref "aspire-integration-testing" >}}) | Integration tests with .NET Aspire fixtures and real dependencies |

## Agents

The four agents are the four middle phases of the pipeline; each can also be invoked directly.

| Agent | Role in the pipeline | Skills it loads |
|---|---|---|
| `coverage-gap-finder` | Builds, tests with coverage, and picks exactly one highest-CRAP method; writes `gap-report.md`. Diagnoses only — never writes tests. | crap-analysis |
| `test-author` | Writes exactly one test file, runs it green, never touches the source under test. Re-invoked as a refactor pass when the reviewer requests changes. | test-type-selection, test-authoring |
| `mutation-validator` | Proves the test catches regressions — Stryker.NET when installed, a single manual mutation otherwise. Must leave source code untouched. | stryker-dotnet |
| `test-reviewer` | Reviews the test file with deliberately fresh eyes (it receives only the file path, no pipeline context) against the two convention skills. | test-authoring, test-type-selection |

The agents communicate through handoff files in a temporary `.claude/add-coverage-work/`
directory — see the [command page]({{< ref "add-coverage" >}}) for the full flow.

## Requirements

- CLI tools on PATH: `dotnet` (.NET 8+), `git`, `gh` (authenticated)
- .NET local tools: `dotnet-reportgenerator-globaltool` (required),
  `dotnet-stryker` (optional — enables Stryker mutation testing)
- Test-project NuGet packages: `xunit`, `Shouldly`, `NSubstitute`, `coverlet.collector`
  (`Microsoft.Playwright` only if an e2e test is warranted)
- `coverage.runsettings` at the repo root

The command's Phase 0 checks all of these and offers to set up what's missing — always with your
approval, never silently.

## Customization

- **CRAP thresholds** (> 30, or 0% coverage with complexity ≥ 5) live in
  `agents/coverage-gap-finder.md` — fork the repo to adjust.
- **Framework conventions** (xUnit + Shouldly + NSubstitute) live in
  `skills/test-authoring/SKILL.md` — fork and edit if your stack uses NUnit, FluentAssertions, or
  Moq.
