# Step 2 — Triage scope

Decide which issue you will actually implement, and whether it fits in one pull request.
Nothing has been claimed yet, so changing course here costs nothing.

If the user explicitly asked you to implement the whole issue regardless of sub-issues or
size, note that in the plan and go to [claim.md](claim.md).

## A. The issue has sub-issues

Use `subIssuesSummary` and `subIssues` from step 1.

| Situation | Action |
|---|---|
| Some sub-issues are open | **Pivot.** Pick the first eligible sub-issue in listed order. Restart at step 1 ([context.md](context.md)) with it; it may have sub-issues of its own. Tell the user you pivoted, and why. |
| Open sub-issues exist, but none is eligible | **Stop.** Report each open sub-issue and what blocks it (an assignee, or an open `blockedBy` issue). Do not fall back to implementing the parent. |
| All sub-issues are closed | Compare the parent's acceptance criteria with what the sub-issues delivered. If nothing remains, tell the user the parent looks ready to close and stop. Otherwise, continue with section B, scoped to the remaining work only. |

A sub-issue is **eligible** when it is open, unassigned or assigned to you, and has no open
`blockedBy` issues.

After a pivot, the pull request closes the sub-issue (`Fixes #child`) and references the
parent (`Relates to #parent`).

## B. Size check (issues without open sub-issues)

Sketch the plan's phases as a short outline in your head or in chat. Do not write the plan
file yet. Then treat the issue as **too big** if either of these is true:

- Two or more phases could each ship on their own as a useful, reviewable pull request
  (for example, a separate API, UI, and data migration).
- The change would not be a pull request a reviewer can finish in one sitting. As a rough
  guide, it would touch more than about 15 files or mix unrelated concerns.

An `epic` label, or a body that is a checklist of separate features, should make you look
hard at the size check. Neither one decides it alone.

If the issue fits, go to [claim.md](claim.md).

## C. Too big: propose a split and stop

Do not claim the issue, create a branch, or write code. Return a proposal to the user:

1. One line on why the issue is too big for one pull request.
2. A numbered list of proposed sub-issues. Give each a title, a one- or two-sentence scope,
   and its acceptance criteria.
3. The dependency order, and which sub-issue to start with.

Ask the user to approve, edit, or reject the split. Creating issues is visible to the whole
repository, so never create them without approval. Once the user approves, follow
[split-issue.md](split-issue.md), then restart at step 1 with the first sub-issue.
