---
name: implement-feature
description: End-to-end workflow for implementing, fixing, or otherwise working on a specific GitHub issue. Fetches full issue context with the gh CLI, produces a phased implementation plan, implements it, gets an independent agent review, verifies build/tests/format, and opens a pull request that references the issue. Use whenever asked to implement, fix, or work on a GitHub issue by number or URL.
metadata:
  author: nimblepros
  version: "1.2"
  spec: agentskills.io
compatibility: Requires git, an authenticated gh CLI, and access to the relevant GitHub repository. The optional configuration resolver needs the .NET 10 SDK.
---

# Implement Feature from GitHub Issue

## Purpose

Provide a repeatable, verifiable workflow for taking a GitHub issue from "assigned" to
"pull request open".

## Use This Skill When

- Asked to implement, fix, resolve, or "work on" a specific GitHub issue (by number or URL).
- Picking up an issue from a milestone or project board.
- Resuming work on an issue that already has a plan in the plan directory (skip to the
  implementation step and follow the existing plan).

## Configuration

Repos differ in where they keep plans and how they verify a build. Resolve both **before
step 4** by running:

```bash
dotnet "${CLAUDE_PLUGIN_ROOT}/scripts/resolve-config.cs"
```

It reads `<repo-root>/.claude/nimblepros.json` (and `NIMBLEPROS_*` environment variables,
which take precedence) and prints the resolved settings with defaults already applied, one
per line:

```
plansDir=.claude/plans
verifyCommand=dotnet build && dotnet test && dotnet format --verify-no-changes
```

If that command cannot be run for any reason, use these defaults and carry on — never stop
to ask where files go:

| Setting | Default |
|---|---|
| `plansDir` | `.claude/plans` |
| `verifyCommand` | `dotnet build && dotnet test && dotnet format --verify-no-changes` |

Throughout the rest of this skill, **"the plan directory"** and **"the verify command"**
mean the values resolved here.

## Workflow

Always follow these steps:

1. Verify the specified GitHub issue exists, is open, and is either unassigned or assigned to you (authenticated user of the `gh` CLI).
2. Self-Assign the issue in GitHub (if you're not already assigned).
3. Gather complete issue context (body, images, comments, linked issues/PRs).
4. Persist a phased implementation plan in the plan directory.
5. Implement, test, and independently review the changes.
6. Verify build, tests, and formatting are clean.
7. Open a pull request that closes the issue.

### 1. Verify issue availability and assignment

Ensure the specified GitHub issue exists and is either unassigned or assigned to you (authenticated user of the `gh` CLI). Use the following commands to check.

First, confirm your authentication status with GitHub and your current username:

```bash
gh auth status
```

Next, pull the full context of the issue (replace `<number>` with the issue number):

```bash
gh issue view <number> --json title,body,labels,assignees,milestone,comments,url,state
```

Verify the issue exists, is open, and is either unassigned or assigned to you.

### 2. Self-Assign the issue

Use `gh` CLI to self-assign the issue (or confirm you are already assigned):

```bash
gh issue edit <number> --add-assignee @me
```

In addition, add a comment to the issue stating that you are taking ownership of it, and include a link to the plan file you will create in the plan directory (see step 4).

Example comment: "I (this user's agent) am taking ownership of this issue and will be implementing it. The implementation plan is documented in `<plan directory>/YYYY-MM-DD-issue-NNN-short-slug.md`."

Finally, if a label exists for `agent-assigned`, add it to the issue. If it does not exist, create it and then add it.

### 3. Gather full issue context

Use the `gh` CLI — never guess at issue content:

- (run in step 1 - do not repeat unless necessary) `gh issue view <number> --json title,body,labels,assignees,milestone,comments,url,state`
- Review **all** comments, not just the body. Later comments often change or narrow scope.
- Download and view every image/attachment referenced in the body or comments
  (`user-images.githubusercontent.com` / `github.com/user-attachments` URLs). Screenshots
  frequently contain the actual acceptance criteria.
- Follow every referenced issue and PR (`#NNN` mentions, "relates to", "blocked by",
  "duplicate of") with `gh issue view` / `gh pr view` and read enough of each to understand
  how it affects scope.
- Note labels (bug vs. enhancement, area labels) and milestone — they signal intent and
  urgency.

If, after all of this, the acceptance criteria are still ambiguous, state your assumptions
explicitly in the plan rather than silently choosing.

### 4. Create and persist an implementation plan

Write a phased, task-based markdown plan to the plan directory **before writing any code**.

- Filename: `YYYY-MM-DD-issue-NNN-short-slug.md` (today's date, the issue number, and a
  short kebab-case summary), matching existing files in that folder.
- Structure the plan as numbered phases, each with a checklist of concrete tasks
  (`- [ ] ...`). Include the issue number and link at the top.
- The plan **must** include:
  - **Automated test requirements** — which behaviors get unit / integration / e2e
    coverage, in which test project(s).
  - **A final review phase** — a comprehensive review of the full change set by a
    **separate agent** (fresh context, not the implementing agent), with a task to address
    all significant findings from that review.
  - **A final verification phase** — the application builds, all tests pass, and formatting
    is clean, confirmed by running the verify command.
- Check the plan boxes off as work proceeds so the plan reflects actual progress.

### 5. Implement the plan

- Work through the phases in order; keep commits scoped and coherent.
- Create a feature branch first (e.g. `issue-NNN-short-slug` or `feature/...`, matching
  recent branch names in the repo) — never commit directly to `main`.
- Follow repo conventions: `.editorconfig`, existing project structure, and any relevant
  skills (test authoring, EF Core patterns, etc.).
- Write the automated tests the plan calls for — tests are part of the implementation, not
  an optional follow-up.

### 6. Independent review

- Launch a **separate review agent** (fresh context) to review the complete diff against
  the issue's acceptance criteria: correctness, missed requirements, test coverage gaps,
  and regressions.
- The review should always scan the changes for **security vulnerabilities**, **performance regressions**, and **accessibility issues**.
- Address **all significant findings**. For findings deliberately not addressed, record the
  reasoning in the plan file.
- If the review produced changes, re-run verification afterward.

### 7. Final verification

Confirm (or re-confirm, if step 6 changed anything):

- The full solution builds with no errors.
- All tests pass.
- The formatter reports nothing to change.

Run the verify command — it covers all three, and must complete cleanly before moving on.
Never skip, weaken, or disable tests to get to green.

### 8. Create the pull request

- Push the branch and open a PR with `gh pr create` targeting the default branch.
- The PR body must include:
  - A summary of all changes, organized by area if the change set is large.
  - The test coverage added and the verification performed.
  - `Fixes #NNN` (or `Closes #NNN`) for each issue the PR resolves, so merging closes
    them. Use `Relates to #NNN` for issues touched but not fully resolved.

### 9. Summarize in chat

End with a chat summary covering:

- What the issue asked for and what was delivered.
- The plan file path, branch name, and PR URL.
- Test coverage added and verification results (build/tests/format).
- Any review findings addressed, assumptions made, or follow-up work deferred.

### 10. Guardrails

- Do not start implementing before the plan file exists in the plan directory.
- Do not claim the work is complete unless the verify command ran cleanly after the last
  code change.
- Do not mark review findings as addressed without actually changing the code or recording
  an explicit reason for skipping them.
- Report failures honestly: if tests fail or the review surfaced unresolved problems, say
  so in the summary instead of papering over it.
