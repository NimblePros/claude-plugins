# playwright-testing

Playwright-focused testing — E2E authoring, issue reproduction, and .NET test refactoring.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install playwright-testing@nimblepros
```

## Skills

| Skill | What it covers |
|---|---|
| `playwright-issue-repro-report` | Turn a GitHub issue into a reproducible Playwright scenario, capture screenshot/video/trace artifacts, and post a structured evidence comment. |
| `playwright-dotnet-refactor` | Clean up generated Playwright .NET codegen drafts into readable xUnit tests with role-based locators and intent-based assertions. |
| `playwright-design-review` | Compare a design asset (image or PDF) against live Playwright screenshots, detect and prioritize visual differences, suggest corrections, and optionally create GitHub issues per finding. |

## Commands

| Command | Arguments | What it does |
|---|---|---|
| `repro-issue` | `<issue-number-or-url>` | Fetches a GitHub issue, drafts a Playwright repro test, runs it, collects artifacts, and posts a structured evidence comment on the issue. |
| `refactor-tests` | `<file-path-or-glob>` | Upgrades locators and assertions in Playwright .NET test files, writes the improvements back to disk, and confirms tests still pass. |
| `design-review` | `<design-file> <app-url> [--viewport WxH] [--auth-state <cookies.json>] [--focus "<section>"] [--create-issues]` | Captures live Playwright screenshots, diffs them against a design asset, produces a prioritized findings report, and optionally files GitHub issues per finding. |

## Agents

| Agent | Purpose |
|---|---|
| `e2e-tester` | Writes, runs, and debugs end-to-end suites (Playwright, Cypress, Selenium) for complete user journeys, including flaky-test and CI-only failure diagnosis. |

## Related plugins

If your stack is .NET, also install
[`dotnet-testing`](https://github.com/NimblePros/claude-plugins/tree/main/plugins/dotnet-testing)
— it carries xUnit conventions, coverage analysis, and mutation testing.
