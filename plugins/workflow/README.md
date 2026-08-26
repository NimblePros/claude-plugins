# workflow

The issue → spec → implementation → review → PR loop, driven through the `gh` CLI.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install workflow@nimblepros
```

## Skills

| Skill | Invoke | What it covers |
|---|---|---|
| `feature-specification` | — | Generates a structured feature specification from a GitHub issue. |
| `implement-feature` | — | End-to-end workflow for an issue: gather full context, persist a phased plan, implement, get an independent agent review, verify, open a PR. |
| `commit` | `/workflow:commit` | Generates a commit message following project conventions. |
| `conventional-commit` | — | Conventional Commits message and PR title format. |
| `pr` | `/workflow:pr` | Full PR lifecycle from investigation through deployment verification. |
| `review` | `/workflow:review` | Reviews local diffs or open PRs with parallel agents, verifies claims, and resolves review threads. |

## Agents

| Agent | Purpose |
|---|---|
| `issue-agent` | Reads backlog items, breaks issues into subtasks, updates issues with checklists. |
| `spec-agent` | Turns GitHub issues into implementation-ready specs committed to the repo. |
| `pr-agent` | Runs final quality checks and opens a draft PR using the repo's PR template. |

## Configuration

Every skill works with no configuration. To override a default, add
`.claude/nimblepros.json` to the repo:

```json
{
  "plansDir": "docs/implementation-plans",
  "specsDir": "docs/specs",
  "verifyCommand": "dotnet BuildTestFormat.cs"
}
```

| Setting | Default | Used by |
|---|---|---|
| `plansDir` | `.claude/plans` | `implement-feature` |
| `specsDir` | `.claude/specs` | `feature-specification` |
| `verifyCommand` | `dotnet build && dotnet test && dotnet format --verify-no-changes` | `implement-feature` |

Every key is optional; omitted keys use the default. Set `NIMBLEPROS_PLANS_DIR`,
`NIMBLEPROS_SPECS_DIR`, or `NIMBLEPROS_VERIFY_COMMAND` to override a value for a single
session — environment variables beat the config file.

Skills read these through `scripts/resolve-config.cs`, which applies the precedence and
prints `key=value` lines. It runs identically under bash and PowerShell, and always exits
`0`: a missing or malformed config falls back to defaults so a workflow never stalls on
configuration.

## Requirements

- `git`
- `gh` (GitHub CLI, authenticated — run `gh auth login`)
- .NET 10 SDK — only for `scripts/resolve-config.cs`. Without it, skills fall back to the
  defaults above.
