---
title: "testing"
description: "Stack-neutral testing practice: end-to-end test authoring, Playwright-based issue reproduction, and design-vs-implementation review"
summary: "Three commands, two skills, and one agent for Playwright-based testing and QA"
date: 2026-08-13
weight: 50
---

Stack-neutral testing practice — nothing here assumes .NET.

```bash
claude plugin install testing@nimblepros
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

## Agents

**`e2e-tester`** writes, runs, and debugs end-to-end suites across Playwright, Cypress, and
Selenium. It follows a discover → design → implement → validate → report loop, with a resilient
selector priority (`data-testid` → ARIA roles → visible text → CSS as last resort), and handles
the messy edge cases: flaky tests, CI-only failures, auth bypass via API or stored state, and
database seeding. It keeps project-scoped memory of framework versions, fixtures, known flaky
tests, and selector conventions across sessions.

The agent and skill are independent — the agent doesn't load the skill — but they pair
naturally: `e2e-tester` builds and maintains suites, while the repro-report skill turns
individual bug reports into evidence.

## Related plugins

If your stack is .NET, install [dotnet-testing]({{< ref "/docs/dotnet-testing" >}}) as well — it
carries the xUnit conventions, coverage analysis, and mutation testing that this plugin
deliberately leaves out. Its [playwright-dotnet-refactor]({{< ref "/docs/dotnet-testing/playwright-dotnet-refactor" >}})
skill is the .NET-specific complement to the Playwright practice here; the
[`/playwright-testing:refactor-tests`]({{< ref "refactor-tests" >}}) command in this plugin
applies that skill in batch over a file or glob.
