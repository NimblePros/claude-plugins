---
description: Reproduce a GitHub issue with Playwright, capture evidence artifacts, and post a structured comment on the issue.
argument-hint: <issue-number-or-url>
allowed-tools: Bash(dotnet *), Bash(git *), Bash(gh *), Write, Edit, Read, Glob
---

<objective>
Run the full issue-repro pipeline for the GitHub issue identified by `$ARGUMENTS`:

1. Fetch issue context from GitHub.
2. Draft or adapt a Playwright repro scenario.
3. Run the scenario and capture evidence artifacts.
4. Post a structured comment on the issue with the repro result and evidence links.

`$ARGUMENTS` must be an issue number (e.g. `42`) or a full GitHub issue URL.
</objective>

<context>
Current branch: !`git branch --show-current`
Repo: !`gh repo view --json nameWithOwner -q .nameWithOwner`
</context>

<process>

## Phase 0 — Verify prerequisites

### 0.1 CLI tools

```bash
dotnet --version
git --version
gh auth status
```

`dotnet` and `git` are hard requirements — stop if either is missing. If `gh auth status` fails, the final comment-posting step will fail; warn the user to run `gh auth login` and continue through Phase 3.

### 0.2 Playwright browser binaries

Identify the E2E test project (glob `**/*.EndToEndTests.csproj` or `**/*E2E*.csproj`). If a matching project is found, ensure Playwright browsers are installed:

```bash
dotnet build <EndToEndTests.csproj>
pwsh <EndToEndTests>/bin/Debug/net*/playwright.ps1 install --with-deps chromium
```

If no E2E project exists, tell the user and stop — this command requires an existing Playwright test project.

---

## Phase 1 — Fetch issue context

```bash
gh issue view $ARGUMENTS --json number,title,body,labels,assignees
```

Extract:

- Issue title and URL.
- Expected behavior and observed behavior (infer from body if not explicit).
- Acceptance clues (labels, linked PRs, reproduction steps).

Write a one-paragraph repro summary to `.claude/repro-work/issue-context.md`. Create the directory if it doesn't exist.

---

## Phase 2 — Build repro scenario

Using the `playwright-issue-repro-report` skill:

1. Convert the issue narrative into deterministic repro steps.
2. Locate the most relevant existing test file (search for keywords from the issue title in `**/*.cs` test files).
3. Write or adapt a focused Playwright C# repro test:
   - Prefer `GetByRole`, `GetByLabel`, `GetByText` locators.
   - Assert only user-observable behavior.
   - Avoid hard sleeps.
4. Write the test path to `.claude/repro-work/repro-test-path.txt`.

---

## Phase 3 — Run the repro and collect artifacts

Run the repro test in isolation:

```bash
dotnet test <EndToEndTests.csproj> --filter "FullyQualifiedName~<TestName>"
```

Collect artifacts from the latest folder under `TestResults/PlaywrightArtifacts/`:

- `failure.png` — screenshot at point of failure or final state.
- `trace.zip` — Playwright trace for step-by-step inspection.
- `video/` — recorded video if available.

Write artifact paths to `.claude/repro-work/artifacts.md`.

Record repro status:

- **Reproduced** — test failed with the same observable symptom as the issue.
- **Not Reproduced** — test passed; document what was checked.
- **Inconclusive** — test errored for unrelated reasons or environment couldn't be verified.

---

## Phase 4 — Post issue comment

Using the template at `skills/playwright-issue-repro-report/templates/issue-comment-template.md`, produce a comment body in `.claude/repro-work/issue-comment.md`.

Include:

- Repro status (bold).
- Environment (branch, commit SHA, browser, app URL).
- Exact steps executed.
- Observed vs. expected result.
- Evidence: attach `failure.png` inline and reference trace/video paths.
- Next action (owner, follow-up test, missing data).

Post the comment:

```bash
gh issue comment $ARGUMENTS --body-file .claude/repro-work/issue-comment.md
```

If `gh auth status` failed in Phase 0, print the comment body to the terminal instead and ask the user to post it manually.

---

## Cleanup

Do not delete `.claude/repro-work/` — it serves as an audit trail. Inform the user of all generated file paths.

</process>
