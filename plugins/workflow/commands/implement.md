---
description: "Implement a GitHub issue end-to-end: fetch context, triage scope, plan, implement, review, and open a PR."
argument-hint: <issue-number-or-url>
allowed-tools: Bash(dotnet *), Bash(git *), Bash(gh *), Write, Edit, Read, Glob
---

<objective>
Execute the `implement-feature` skill for the GitHub issue identified by `$ARGUMENTS`:

1. Fetch full issue context, including comments, attachments, the parent issue, and sub-issues.
2. Triage scope. If the issue has open sub-issues, pivot to the first eligible one. If it is
   too big for one PR, stop and propose a split.
3. Create a phased implementation plan in the plan directory.
4. Implement, test, and independently review the changes.
5. Verify build, tests, and formatting are clean.
6. Open a pull request that closes the issue.

The issue you implement may differ from `$ARGUMENTS`. A run may also end at step 2 with a
split proposal instead of a pull request.

`$ARGUMENTS` must be an issue number (e.g. `42`) or a full GitHub issue URL.
</objective>

<context>
Current branch: !`git branch --show-current`
Repo: !`gh repo view --json nameWithOwner -q .nameWithOwner`
</context>

<process>

## Phase 0 — Verify prerequisites

```bash
dotnet --version
git --version
gh auth status
```

`dotnet` and `git` are hard requirements — stop if either is missing.
If `gh auth status` fails, stop and ask the user to run `gh auth login`.
If `$ARGUMENTS` is empty, print usage and stop.

---

## Phases 1–6

Follow the `implement-feature` skill.

</process>
