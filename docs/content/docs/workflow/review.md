---
title: "review"
description: "Review local diffs or PRs with parallel agents and confidence-scored findings"
summary: "/workflow:review — five independent review lenses over one diff"
date: 2026-08-13
weight: 60
---

> Review code changes using parallel agents. Works on local diffs or open PRs. Checks project
> conventions, scans for bugs, verifies claims, and resolves review threads. Use when the user
> says "review", "review this", "review PR", "check this PR", or invokes /review.

Invoke with `/workflow:review`, optionally passing a PR number. Without one it reviews the local
diff against `origin/main` — a PR is *not* required, and the skill explicitly forbids creating
one just to run a review.

## What it covers

A five-step fan-out review:

1. **Pick the target** — a PR (via `gh pr view` / `gh pr diff`) or the local diff.
2. **Dispatch five independent agents** over the same diff: **Bug Scan** (logic errors only —
   style and type errors are CI's job), **Git History Context** (is the change consistent with
   the file's evolution?), **Shared Code Impact** (all callers of every modified file — called
   out as "the #1 cause of errors that make it into production"), **Project Convention Check**
   (branch naming, conventional PR title under 65 chars), and **Security and Secrets Check**
   (hardcoded secrets, OWASP Top 10, authn/authz changes).
3. **Score every finding 0–100 for confidence** — only findings scoring 75+ are reported,
   because "false positives erode trust in the review process."
4. **Report** in a fixed format with evidence and file/line links.
5. **PR only:** list unresolved review threads via the GitHub GraphQL API and resolve the ones
   that are verifiably fixed — never on assumption.

## Cross-references

This is the "independent review by a separate agent" phase that
[implement-feature]({{< ref "implement-feature" >}}) requires before opening a PR. Its convention
check enforces the same title rules as [pr]({{< ref "pr" >}}) and
[conventional-commit]({{< ref "conventional-commit" >}}). It reads the project's `CLAUDE.md` —
conventions stated there count as high-confidence findings. Requires `git` and an authenticated
`gh` (including `gh api graphql` for thread resolution). No supporting files.
