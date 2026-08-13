---
title: "testing"
description: "Stack-neutral testing practice: end-to-end test authoring and Playwright-based issue reproduction reports"
summary: "Testing practice that doesn't assume .NET"
date: 2026-08-13
weight: 50
---

Stack-neutral testing practice — nothing here assumes .NET.

```bash
claude plugin install testing@nimblepros
```

## Skills

| Skill | One-line summary |
|---|---|
| [playwright-issue-repro-report]({{< ref "playwright-issue-repro-report" >}}) | Turn a GitHub bug report into a Playwright repro with screenshot/video/trace evidence and a structured issue comment |

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
skill is the .NET-specific complement to the Playwright practice here.
