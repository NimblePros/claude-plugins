---
title: "implement-feature"
description: "The end-to-end workflow for implementing a GitHub issue, from context gathering to PR"
summary: "Issue in, reviewed PR out"
date: 2026-08-13
weight: 20
---

> End-to-end workflow for implementing, fixing, or otherwise working on a specific GitHub issue.
> Fetches full issue context with the gh CLI, produces a phased implementation plan in
> .claude/plans, implements it, gets an independent agent review, verifies build/tests/format,
> and opens a pull request that references the issue. Use whenever asked to implement, fix, or
> work on a GitHub issue by number or URL.

Invoke with `/workflow:implement <issue-number-or-url>`.

## What it covers

The plugin's flagship seven-step loop:

1. **Gather context exhaustively** — `gh issue view` with all fields, read *every* comment,
   download referenced screenshots and attachments (they "frequently contain the actual
   acceptance criteria"), follow every linked issue and PR.
2. **Plan before code** — persist a phased plan at `.claude/plans/YYYY-MM-DD-issue-NNN-slug.md`
   before writing anything. Resuming an issue that already has a plan skips straight to
   implementation.
3. **Implement on a feature branch** — never `main`; write the tests the plan calls for.
4. **Independent review** — launch a separate review agent with fresh context and address all
   significant findings.
5. **Final verification** — build, tests, formatting all clean.
6. **Open the PR** — `gh pr create` with `Fixes #NNN` / `Closes #NNN` (or `Relates to #NNN` for
   partial work).
7. **Summarize** — plan path, branch, PR URL, and any deferred work.

Its guardrails are worth quoting: "Never skip, weaken, or disable tests to get to green" and
"Report failures honestly."

## Cross-references

Step 4's independent review is exactly what the [review]({{< ref "review" >}}) skill provides.
The plan format parallels [feature-specification]({{< ref "feature-specification" >}}) (but plans
live in `.claude/plans/`, specs in `.claude/specs/`). The skill directs Claude to load any
relevant domain skills during implementation — e.g.
[test-authoring]({{< ref "/dotnet-testing/test-authoring" >}}) from `dotnet-testing` on a
.NET project. Its verification examples reference a .NET build/format harness; substitute your
own stack's equivalent. Requires `git` and an authenticated `gh`. Also triggers whenever you ask Claude to implement
or fix an issue by number or URL without the slash command.
