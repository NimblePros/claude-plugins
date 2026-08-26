---
title: "test-authoring"
description: "xUnit conventions: naming, Shouldly, NSubstitute, fixtures, and meaningful tests"
summary: "The house style for .NET tests"
date: 2026-08-13
weight: 30
---

> Conventions for writing xUnit tests. Covers naming, assertion style (Shouldly), mocking
> (NSubstitute), fixture patterns, and the rules that make tests meaningful vs. ornamental. Load
> this skill before writing or reviewing any test file.

## What it covers

- **Naming** — test classes `<Subject>_<Scenario>` (a separate class per distinct outcome),
  methods `<ExpectedOutcome>_<Condition>`.
- **Structure** — Arrange-Act-Assert inline, no constructor setup, one behaviour per test, one
  class per file.
- **Assertions and mocking** — Shouldly over bare `Assert.*`; NSubstitute over Moq or hand-rolled
  fakes.
- **Fixtures** — `file static class` fixtures with a `Make()` factory carrying sensible defaults.
- **The most important rule** — "a test that passes when the code is broken is worse than no
  test." The acceptance question: would deleting the key source line make this test fail? Good
  assertion targets (return values, public state, `.Received()` calls) vs. bad ones (private
  fields, framework behaviour, tautologies).
- **Placement ladder** — alongside existing tests for the class → the project with the
  `<ProjectReference>` → naming convention → stop and ask rather than guess.

## Cross-references

Tells the reader to load [test-type-selection]({{< ref "test-type-selection" >}}) *first* to
decide what kind of test to write. Both the `test-author` and `test-reviewer` agents in the
[`/add-coverage`]({{< ref "add-coverage" >}}) pipeline load it — the reviewer's checklist cites
this skill's sections by name, and reviewers are forbidden from raising stylistic preferences not
found in it. If your stack uses NUnit, FluentAssertions, or Moq instead, fork the repo and edit
this skill — it is the single place the framework conventions live.
