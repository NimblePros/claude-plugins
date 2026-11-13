# Step 6 — Open the pull request and summarize

## Pull request

Push the branch and open a PR with `gh pr create`, targeting the default branch. The PR body
must include:

- A summary of all changes, organized by area if the change set is large.
- The test coverage added and the verification performed.
- `Fixes #NNN` (or `Closes #NNN`) for each issue the PR resolves, so merging closes them.
- `Relates to #NNN` for issues it touches but does not fully resolve. That includes the
  parent issue after a pivot to a sub-issue.

## Chat summary

End with a summary in chat covering:

- What the issue asked for and what was delivered.
- **Triage outcome**: whether you pivoted from a parent issue, and if so from which one
  and why.
- The plan file path, the branch name, and the PR URL.
- The test coverage added, and the verification results (build, tests, format).
- Review findings addressed, assumptions made, and any follow-up work deferred.
