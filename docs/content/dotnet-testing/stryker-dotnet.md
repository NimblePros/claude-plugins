---
title: "stryker-dotnet"
description: "Scoped Stryker.NET mutation testing runs and how to interpret them"
summary: "Prove a test suite has teeth, in seconds not minutes"
date: 2026-08-13
weight: 50
---

> Run Stryker.NET mutation testing against a specific .NET source file. Covers installation
> (local tool), scoped invocation, JSON output parsing, and interpreting killed/survived results
> to decide pass/fail.

Invoke standalone with `/dotnet-testing:mutation-test <source-file-or-class-name>`.

## What it covers

Mutation-testing mechanics — the five mutant statuses (Killed, Survived, NoCoverage, Timeout,
CompileError) and the score formula `Killed / (Killed + Survived + Timeout) × 100` — but its
practical core is **scoped invocation**: run from the test project directory with
`--mutate "**/TargetClass.cs"` and `--test-case-filter`, collapsing a multi-minute full-suite run
into seconds. It documents the JSON report shape, ready-to-paste parsing snippets, and a
pass/fail rubric: ≥ 80% score passes; below 80%, any survived mutant, or zero mutants generated
(a scoping problem) fails.

Installation is a pinned local tool (`dotnet-stryker` in `.config/dotnet-tools.json` with
`rollForward: false`), with a detection snippet and instructions for falling back to a manual
mutation when Stryker isn't available.

## Cross-references

The knowledge behind Phase 3 of [`/add-coverage`]({{< ref "add-coverage" >}}) — the
`mutation-validator` agent loads it and implements both the Stryker path and the manual-mutation
fallback the skill describes. Complements [crap-analysis]({{< ref "crap-analysis" >}}): CRAP
finds code that *lacks* tests; mutation testing proves the tests you add actually catch
regressions. Single-file skill.
