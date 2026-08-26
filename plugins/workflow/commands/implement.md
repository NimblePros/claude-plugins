---
description: "Implement a GitHub issue end-to-end: fetch context, plan, implement, review, and open a PR."
argument-hint: <issue-number-or-url>
allowed-tools: Bash(dotnet *), Bash(git *), Bash(gh *), Write, Edit, Read, Glob
---

<objective>
Execute the `implement-feature` skill for the GitHub issue identified by `$ARGUMENTS`:

1. Fetch full issue context including comments and attachments.
2. Create a phased implementation plan in `.claude/plans/`.
3. Implement, test, and independently review the changes.
4. Verify build, tests, and formatting are clean.
5. Open a pull request that closes the issue.

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

## Phases 1–5

Follow the `implement-feature` skill.

</process>
