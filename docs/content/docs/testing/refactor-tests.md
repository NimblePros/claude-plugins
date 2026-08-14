---
title: "/refactor-tests command"
description: "Upgrade Playwright .NET test files to semantic locators and intent-based assertions, confirmed green"
summary: "Batch-refactor codegen noise into maintainable xUnit tests"
date: 2026-08-14
weight: 40
---

> Refactor generated or messy Playwright .NET test files into clean, maintainable xUnit tests
> with role-based locators and intent-based assertions.

```bash
/playwright-testing:refactor-tests tests/E2E/LoginTests.cs
/playwright-testing:refactor-tests "tests/E2E/**/*Tests.cs"
# omit argument to auto-discover all Playwright test files in the workspace
```

## The pipeline

| Phase | What happens |
|---|---|
| 0. Prerequisites | Verifies `dotnet`; resolves `$ARGUMENTS` to a list of Playwright C# test files |
| 1. Baseline run | Builds and runs the targets to confirm their current green state; stops (with permission) if any are already red |
| 2. Refactor each file | Applies the six refactor rules (see below); writes improvements back to disk; logs a one-line change summary per file |
| 3. Verify green | Re-runs the same tests; reverts any file that becomes newly red and adds it to a manual-review list |
| 4. Report | Prints a summary: files refactored, key improvements, files skipped or reverted |

## Refactor rules

1. Preserve user intent — no silently added or removed scenario steps.
2. Upgrade locators: `GetByRole` → `GetByLabel`/`GetByPlaceholder` → `GetByText` → `GetByTestId`; CSS/XPath only as last resort.
3. Write readable xUnit — obvious Arrange-Act-Assert, intent-revealing test names.
4. Extract helpers only when readability genuinely improves.
5. Assert only user-observable behaviour — never internal IDs, CSS class internals, or hidden DOM.
6. Deterministic waits via Playwright assertions — no `await Task.Delay`.

## What it leans on

Built on the [`playwright-dotnet-refactor`]({{< ref "/docs/dotnet-testing/playwright-dotnet-refactor" >}})
skill. That skill defines the rules, the output contract, and the rationale format. This command
applies them in batch over a file or glob, wraps the run with a green-gate, and surfaces a
summary rather than inline reasoning.

## Handoff files

| File | Contents |
|---|---|
| `.claude/refactor-work/baseline.txt` | Pass/fail counts before refactoring |
| `.claude/refactor-work/changes.md` | One-line change log per file, plus any assumptions |
