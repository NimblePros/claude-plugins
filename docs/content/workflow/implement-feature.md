---
title: "implement-feature"
description: "The end-to-end workflow for implementing a GitHub issue, from context gathering to PR"
summary: "Issue in, reviewed PR out"
date: 2026-08-13
weight: 20
---

> End-to-end workflow for implementing, fixing, or otherwise working on a specific GitHub issue.
> Fetches full issue context with the gh CLI, triages scope (pivoting to an open sub-issue, or
> proposing a split when an issue is epic-sized), produces a phased implementation plan,
> implements it, gets an independent agent review, verifies build/tests/format, and opens a pull
> request that references the issue.

Invoke with `/workflow:implement <issue-number-or-url>`.

## What it covers

The plugin's flagship loop:

1. **Gather context exhaustively**: `gh issue view` with all fields, *every* comment, referenced
   screenshots and attachments (they often contain the real acceptance criteria), and every
   linked issue and PR. A GraphQL query also fetches the issue's parent and sub-issues.
2. **Triage scope**: before claiming anything, decide which issue to implement.
   - **Open sub-issues**: pivot to the first eligible one (open, unassigned or yours, and not
     blocked), and restart the workflow on it. The PR then says `Fixes #child` and
     `Relates to #parent`.
   - **Too big for one PR**: stop before branching and propose a split, with titles, scope,
     acceptance criteria, and order. After you approve, it creates the sub-issues, links them
     to the parent natively, and starts on the first.
   - **Fits**: carry on.
3. **Claim**: self-assign, comment with the plan path, and add the `agent-assigned` label.
4. **Plan before code**: persist a phased plan at
   `<plansDir>/YYYY-MM-DD-issue-NNN-slug.md` before writing anything. Resuming an issue that
   already has a plan skips straight to implementation.
5. **Implement, review, verify**: work on a feature branch (never `main`) and write the tests
   the plan calls for. A separate review agent with fresh context checks the change, and all
   significant findings get addressed. Build, tests, and formatting must be clean.
6. **Open the PR and summarize**: `gh pr create` with `Fixes #NNN`, then a chat summary that
   includes the triage outcome.

The skill uses progressive disclosure. `SKILL.md` holds only the step list and guardrails, and
the agent reads each step's file under `references/` when it reaches that step. A triage-only
run, such as "is #42 ready to work on?", never loads the implementation details.

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
