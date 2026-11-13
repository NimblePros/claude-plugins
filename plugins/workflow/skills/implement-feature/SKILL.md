---
name: implement-feature
description: End-to-end workflow for implementing, fixing, or otherwise working on a specific GitHub issue. Fetches full issue context with the gh CLI, triages scope (pivoting to an open sub-issue, or proposing a split when an issue is epic-sized), produces a phased implementation plan, implements it, gets an independent agent review, verifies build/tests/format, and opens a pull request that references the issue. Use whenever asked to implement, fix, or work on a GitHub issue by number or URL, or to check whether an issue or epic is ready to implement.
metadata:
  author: nimblepros
  version: "1.3"
  spec: agentskills.io
compatibility: Requires git, an authenticated gh CLI, and access to the relevant GitHub repository. The optional configuration resolver needs the .NET 10 SDK.
---

# Implement Feature from GitHub Issue

Take a GitHub issue from "open" to "pull request open", but only after confirming that it
is the right issue to implement and that it fits in one pull request.

## Use This Skill When

- Asked to implement, fix, resolve, or "work on" a specific GitHub issue (by number or URL).
- Picking up an issue from a milestone or project board.
- Asked whether an issue or epic is ready to work on, or how to break it up. Only steps
  1–2 apply.
- Resuming an issue that already has a plan in the plan directory. Skip to step 5 and
  follow the existing plan.

## Workflow

Run the steps in order. Read each step's file when you reach that step, not before.

| # | Step | Details |
|---|------|---------|
| 1 | Verify the issue and gather full context, including parent and sub-issues | [references/context.md](references/context.md) |
| 2 | **Triage scope**: implement it, pivot to a sub-issue, or stop and propose a split | [references/triage.md](references/triage.md) |
| 3 | Claim the issue (assign, comment, label) | [references/claim.md](references/claim.md) |
| 4 | Persist a phased implementation plan | [references/plan.md](references/plan.md) |
| 5 | Implement, get an independent review, and verify | [references/implement.md](references/implement.md) |
| 6 | Open the pull request and summarize in chat | [references/pull-request.md](references/pull-request.md) |

Step 2 often ends the run: it may hand off to a sub-issue (restart at step 1 with that
issue) or return a split proposal to the user. Never claim an issue before triage has
settled which issue you are implementing.

## Guardrails

- Never implement a parent issue while it has open sub-issues, unless the user explicitly
  asks for the whole parent.
- Never create, edit, or link issues during triage without the user's approval.
- Do not start implementing before the plan file exists in the plan directory.
- Do not claim the work is complete unless the verify command ran cleanly after the last
  code change. Never skip, weaken, or disable tests to get to green.
- Do not mark review findings as addressed without changing the code or recording an
  explicit reason for skipping them.
- Report failures honestly. If tests fail, the review surfaced unresolved problems, or
  triage changed the target issue, say so in the summary.
