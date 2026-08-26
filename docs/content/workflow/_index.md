---
title: "workflow"
description: "Issue-to-PR development workflow over the gh CLI: feature specs, implementation plans, commits, code review, and pull requests"
summary: "The issue → spec → implementation → review → PR loop"
date: 2026-08-13
weight: 70
---

The issue → spec → implementation → review → PR loop, driven through the `gh` CLI.

```bash
claude plugin install workflow@nimblepros
```

## Two ways through the loop

This plugin offers the same journey at two granularities:

- **The agent pipeline** — [`issue-agent`](#agents) breaks an issue into subtasks →
  [`spec-agent`](#agents) writes a spec to `.claude/specs/` using the
  [feature-specification]({{< ref "feature-specification" >}}) skill → you (or another agent)
  implement → [review]({{< ref "review" >}}) checks the result → [`pr-agent`](#agents) opens a
  draft PR.
- **The single-skill path** — [implement-feature]({{< ref "implement-feature" >}}) collapses the
  middle of that loop into one workflow: gather full issue context, persist a phased plan in
  `.claude/plans/`, implement, get an independent agent review, verify, open a PR.

## Skills

| Skill | Invoke | One-line summary |
|---|---|---|
| [feature-specification]({{< ref "feature-specification" >}}) | — | The structured spec format written to `.claude/specs/`, with phased checklists and verifiable "Done When" criteria |
| [implement-feature]({{< ref "implement-feature" >}}) | `/workflow:implement` | The end-to-end issue workflow: context → plan → implement → independent review → verify → PR |
| [commit]({{< ref "commit" >}}) | `/workflow:commit` | Conversation-aware commit messages following Conventional Commits |
| [pr]({{< ref "pr" >}}) | `/workflow:pr` | Full PR lifecycle: context, merge-from-main validation, templated `gh pr create`, verification |
| [review]({{< ref "review" >}}) | `/workflow:review` | Five parallel review agents over a local diff or PR, with confidence-scored findings |

## Reference

| Page | What it is |
|---|---|
| [conventional-commit]({{< ref "conventional-commit" >}}) | The Conventional Commits format reference: type taxonomy, scope, SemVer impact, and the PR-title format. Not an installable skill — loaded by `commit` and cited by `pr-agent` and the [pr]({{< ref "pr" >}}) skill. |

## Agents

| Agent | Purpose |
|---|---|
| `issue-agent` | Reads backlog items with `gh issue view`, decomposes them into `- [ ]` checklists written back to the issue, creates linked sub-issues, and hands scope off to `spec-agent`. |
| `spec-agent` | Turns a GitHub issue into an implementation-ready spec in `.claude/specs/`, delegating document structure to the [feature-specification]({{< ref "feature-specification" >}}) skill. |
| `pr-agent` | The terminal gate: runs build and tests, confirms every acceptance criterion in both the spec and the issue checklist, then opens a **draft** PR (using the [conventional-commit]({{< ref "conventional-commit" >}}) title format) and leaves it draft for developer review. |

## Where files land

Two persistence directories, easy to confuse:

- `.claude/specs/<issue-number>-<short-title>.md` — specs, from `spec-agent` /
  [feature-specification]({{< ref "feature-specification" >}})
- `.claude/plans/YYYY-MM-DD-issue-NNN-short-slug.md` — implementation plans, from
  [implement-feature]({{< ref "implement-feature" >}})

## Requirements

- `git`
- `gh` (GitHub CLI, authenticated — run `gh auth login`)

Some skills carry .NET-flavored examples (build/test verification commands), but the workflow
itself is language-agnostic — substitute your own build and test commands.
