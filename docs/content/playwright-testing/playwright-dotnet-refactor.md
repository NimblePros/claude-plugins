---
title: "playwright-dotnet-refactor"
description: "Refactor generated Playwright .NET drafts into readable, resilient xUnit tests"
summary: "From codegen noise to intent-revealing tests"
date: 2026-08-13
weight: 25
aliases: ["/dotnet-testing/playwright-dotnet-refactor/"]
---

> Refactor generated Playwright .NET test drafts into readable xUnit tests with role-based
> locators, intent-based assertions, and minimal helper extraction. Use when a generated
> Playwright test is noisy, brittle, or hard to maintain and needs cleanup without changing user
> intent.

## What it covers

A cleanup pass for Playwright codegen output, governed by six rules:

1. Preserve user intent exactly — no silently added or removed scenario steps.
2. Semantic locators in strict priority order: `GetByRole` → `GetByLabel`/`GetByPlaceholder` →
   `GetByText` → `GetByTestId`; avoid CSS/XPath.
3. Readable xUnit with obvious Arrange-Act-Assert.
4. Extract helpers only when readability genuinely improves.
5. Assert only user-observable behaviour — never internal IDs or CSS internals.
6. Deterministic waits via Playwright assertions — no arbitrary sleeps.

The output contract requires a rationale alongside the refactored code: what locators changed,
what assertions changed, what was extracted, and what was assumed.

## Ships with

| File | Contents |
|---|---|
| `assets/refactor-prompt-template.md` | A reusable prompt with slots for scenario intent, the generated draft, and constraints |

## Cross-references

The [`/playwright-testing:refactor-tests`]({{< ref "refactor-tests" >}}) command applies this
skill in batch over a file or glob. It shares Playwright ground with the stack-neutral
[playwright-issue-repro-report]({{< ref "playwright-issue-repro-report" >}}) skill in this
plugin: that one *produces* repro tests from bug reports; this one *cleans up* generated tests
into maintainable xUnit. It is not wired into the
[`/dotnet-testing:add-coverage`]({{< ref "/dotnet-testing/add-coverage" >}}) pipeline.
