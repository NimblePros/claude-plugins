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
| `implement-feature` | — | End-to-end workflow for an issue: gather full context, persist a phased plan in `.claude/plans`, implement, get an independent agent review, verify, open a PR. |
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

## Requirements

- `git`
- `gh` (GitHub CLI, authenticated — run `gh auth login`)
