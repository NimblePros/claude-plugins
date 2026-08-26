---
title: "crap-analysis"
description: "CRAP scoring to find the code most likely to break: high complexity, low coverage"
summary: "Change Risk Anti-Patterns via OpenCover + ReportGenerator"
date: 2026-08-13
weight: 20
---

> Analyze code coverage and CRAP (Change Risk Anti-Patterns) scores to identify high-risk code.
> Use OpenCover format with ReportGenerator for Risk Hotspots showing cyclomatic complexity and
> untested code paths.

Invoke standalone with `/dotnet-testing:crap-analysis [project-or-namespace-filter]`.

## What it covers

The CRAP formula — `comp(m)² × (1 − cov(m)/100)³ + comp(m)` — and how to read it: under 5 is
low risk, 5–30 medium, over 30 means the method needs tests or refactoring. A key insight the
skill demonstrates with a worked example: a complexity-80 method scores 80 even at *100%*
coverage — CRAP flags complexity that coverage alone can't fix.

Practically, it provides:

- A complete `coverage.runsettings` — **OpenCover format is mandatory** (cobertura carries no
  complexity metrics) — with the exclusion set for test/benchmark/migration assemblies and
  generated code.
- ReportGenerator setup as a pinned local tool, the collect → report command sequence, and how to
  read the Risk Hotspots table.
- Recommended thresholds: line > 80%, branch > 60%, CRAP < 30 for new code (relaxed to 60/40 for
  legacy) — and the firm opinion that thresholds are never lowered because code is "too hard to
  test"; you refactor instead.

## Cross-references

This skill is the knowledge behind Phase 1 of
[`/add-coverage`]({{< ref "add-coverage" >}}) — the `coverage-gap-finder` agent loads it, and the
command's environment check uses it as the source of truth for `coverage.runsettings` and the
ReportGenerator tool entry. External tools: `dotnet test` with coverlet, and
`dotnet-reportgenerator-globaltool`. Single-file skill.
