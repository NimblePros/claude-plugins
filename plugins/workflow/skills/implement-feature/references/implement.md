# Step 5 — Implement, review, and verify

## Implement

- Create a feature branch first (for example `issue-NNN-short-slug` or `feature/...`,
  matching recent branch names in the repo). Never commit directly to `main`.
- Work through the plan's phases in order. Keep commits scoped and coherent.
- Follow repo conventions: `.editorconfig`, the existing project structure, and any relevant
  skills (test authoring, EF Core patterns, and so on).
- Write the automated tests the plan calls for. Tests are part of the implementation, not
  an optional follow-up.

## Independent review

- Launch a **separate review agent** with fresh context. Have it review the complete diff
  against the issue's acceptance criteria for correctness, missed requirements, test
  coverage gaps, and regressions.
- The review must also scan for **security vulnerabilities**, **performance
  regressions**, and **accessibility issues**.
- Address **all significant findings**. For any finding you deliberately leave, record the
  reasoning in the plan file.

## Final verification

Run the verify command after the last code change, including any changes from the review.
It must show that:

- the full solution builds with no errors,
- all tests pass, and
- the formatter reports nothing to change.

Next: [pull-request.md](pull-request.md).
