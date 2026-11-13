# Step 1 — Verify the issue and gather context

Use the `gh` CLI for everything. Never guess at issue content.

## Verify availability

```bash
gh auth status
gh issue view <number> --json title,body,labels,assignees,milestone,comments,url,state
```

Continue only if the issue exists, is open, and is unassigned or assigned to the
authenticated `gh` user. Otherwise stop and report why.

## Read the issue hierarchy

`gh issue view` does not return sub-issues or the parent, so query them separately:

```bash
gh api graphql -F owner='{owner}' -F repo='{repo}' -F number=<number> \
  -F query=@"<skill-dir>/scripts/issue-hierarchy.graphql"
```

`<skill-dir>` is the directory that contains this skill's `SKILL.md`. Use its absolute path.
Run the command from inside the target repository; `gh` fills in `{owner}` and `{repo}`. The
result includes:

- `subIssuesSummary`: the total and completed sub-issue counts. Step 2 uses these.
- `subIssues`: the state, assignees, and `blockedBy` dependencies of each sub-issue, in
  the order the issue's author gave them.
- `parent`: if the issue is itself a sub-issue, read the parent with `gh issue view` for
  context. Do not take on the parent's scope.

## Gather the rest of the context

- Read **all** comments, not just the body. Later comments often change or narrow scope.
- Download and view every image or attachment in the body or comments
  (`user-images.githubusercontent.com` / `github.com/user-attachments` URLs). Screenshots
  often contain the real acceptance criteria.
- Follow every referenced issue and PR (`#NNN`, "relates to", "blocked by", "duplicate
  of") with `gh issue view` / `gh pr view`, and read enough to see how each affects scope.
- Note labels (bug vs. enhancement, area, `epic`) and the milestone. They signal intent
  and urgency.

If the acceptance criteria are still ambiguous after all of this, record your assumptions
explicitly in the plan rather than silently choosing.

Next: [triage.md](triage.md).
