---
title: "playwright-testing"
description: "Playwright-focused testing: E2E test authoring, issue reproduction reports, design-vs-implementation review, and refactoring generated .NET Playwright tests"
summary: "Three commands, three skills, and one agent for Playwright-based testing and QA"
date: 2026-08-13
weight: 50
aliases: ["/testing/"]
---

Playwright-focused testing practice. Most content is stack-neutral; the
[playwright-dotnet-refactor]({{< ref "playwright-dotnet-refactor" >}}) skill and the
[`/playwright-testing:refactor-tests`]({{< ref "refactor-tests" >}}) command that applies it are
.NET-specific.

```bash
claude plugin install playwright-testing@nimblepros
```

## Commands

| Command | Arguments | One-line summary |
|---|---|---|
| [`/playwright-testing:repro-issue`]({{< ref "repro-issue" >}}) | `<issue-number-or-url>` | Fetch a GitHub issue, build a Playwright repro, collect artifacts, and post an evidence comment |
| [`/playwright-testing:refactor-tests`]({{< ref "refactor-tests" >}}) | `<file-or-glob>` | Upgrade Playwright .NET tests to semantic locators and intent-based assertions, confirmed green |
| [`/playwright-testing:design-review`]({{< ref "design-review" >}}) | `<design-file> <url> [options]` | Diff a design asset against live Playwright screenshots and produce a prioritized findings report |

## Skills

| Skill | One-line summary |
|---|---|
| [playwright-issue-repro-report]({{< ref "playwright-issue-repro-report" >}}) | Turn a GitHub bug report into a Playwright repro with screenshot/video/trace evidence and a structured issue comment |
| [playwright-design-review]({{< ref "playwright-design-review" >}}) | Compare a design asset (image or PDF) against live Playwright screenshots, classify differences by severity, and optionally create GitHub issues per finding |
| [playwright-dotnet-refactor]({{< ref "playwright-dotnet-refactor" >}}) | Clean Playwright .NET codegen drafts into readable xUnit tests with role-based locators and intent-based assertions |

## Agents

**`e2e-tester`** writes, runs, and debugs end-to-end suites across Playwright, Cypress, and
Selenium. It follows a discover → design → implement → validate → report loop, with a resilient
selector priority (`data-testid` → ARIA roles → visible text → CSS as last resort), and handles
the messy edge cases: flaky tests, CI-only failures, auth bypass via API or stored state, and
database seeding. It keeps project-scoped memory of framework versions, fixtures, known flaky
tests, and selector conventions across sessions.

The agent and skills are independent — the agent doesn't load any skill — but they pair
naturally: `e2e-tester` builds and maintains suites, while
[`playwright-issue-repro-report`]({{< ref "playwright-issue-repro-report" >}}) turns individual
bug reports into evidence and
[`playwright-design-review`]({{< ref "playwright-design-review" >}}) audits implementations
against design assets.

## Related plugins

If your stack is .NET, install [dotnet-testing]({{< ref "/dotnet-testing" >}}) as well — it
carries the xUnit conventions, coverage analysis, and mutation testing that this plugin
deliberately leaves out.
