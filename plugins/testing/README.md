# testing

Stack-neutral testing practice — nothing here assumes .NET.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install testing@nimblepros
```

## Skills

| Skill | What it covers |
|---|---|
| `playwright-issue-repro-report` | Turn a GitHub issue into a reproducible Playwright scenario, capture screenshot/video/trace artifacts, and post a structured evidence comment. |

## Agents

| Agent | Purpose |
|---|---|
| `e2e-tester` | Writes, runs, and debugs end-to-end suites (Playwright, Cypress, Selenium) for complete user journeys, including flaky-test and CI-only failure diagnosis. |

## Related plugins

If your stack is .NET, install [`dotnet-testing`](../dotnet-testing/) as well — it carries the
xUnit conventions, coverage analysis, and mutation testing that this plugin deliberately leaves
out.
