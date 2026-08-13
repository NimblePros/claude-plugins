---
title: "pr"
description: "Full pull-request lifecycle from investigation through deployment verification"
summary: "/workflow:pr — context, validation, templated creation, verification"
date: 2026-08-13
weight: 50
---

> Full lifecycle for creating a pull request, from investigation through deployment
> verification. Use when the user says "create PR", "make PR", "open PR", "ship this", or
> invokes /pr.

Invoke with `/workflow:pr`.

## What it covers

Four phases:

1. **Gather context** — confirm the target branch (default `main`), extract work item IDs from
   the branch name, review `git log` and `git diff --stat` against the target.
2. **Validate** — `git fetch origin main` and **merge** origin/main into the branch. A hard rule:
   always merge, never rebase (rebase breaks build-tag traceability).
3. **Create** — `gh pr create` with a body from the shipped template and a conventional-commit
   title capped at **65 characters**.
4. **Verify** — open the PR in the browser to confirm.

Standing rules: keep descriptions factual, always link work items, and never force-push or push
to main without explicit confirmation.

## Ships with

| File | Contents |
|---|---|
| `references/pr-template.md` | The PR body template (Description, Related Issues, Checklist, Notes for Reviewers) and a copy-paste `gh pr create` invocation |

## Cross-references

Title format comes from [conventional-commit]({{< ref "conventional-commit" >}}); the
[review]({{< ref "review" >}}) skill checks the same 65-character conventional-title rule from
the reviewing side. This skill differs from the `pr-agent` on purpose: the agent is the end of
the spec pipeline and always opens a **draft** PR after verifying acceptance criteria, while
`/workflow:pr` is the interactive path for shipping whatever branch you're on. Requires `git`
and an authenticated `gh`.
