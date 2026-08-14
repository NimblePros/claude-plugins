---
description: Refactor generated or messy Playwright .NET test files into clean, maintainable xUnit tests with role-based locators and intent-based assertions.
argument-hint: <path-to-test-file-or-glob>
allowed-tools: Bash(dotnet *), Bash(git *), Write, Edit, Read, Glob
---

<objective>
Apply the `playwright-dotnet-refactor` skill to the test file(s) identified by `$ARGUMENTS`, write the improvements back to disk, and confirm the tests still pass green.

`$ARGUMENTS` should be a path to a C# test file or a glob (e.g. `tests/EndToEndTests/LoginTests.cs` or `tests/EndToEndTests/**/*Tests.cs`).
If omitted, locate all Playwright C# test files in the workspace (glob `**/*Tests.cs` filtered to files containing `IPage` or `IBrowserContext`).
</objective>

<context>
Current branch: !`git branch --show-current`
</context>

<process>

## Phase 0 — Verify prerequisites

### 0.1 CLI tools

```bash
dotnet --version
```

Stop if `dotnet` is missing.

### 0.2 Identify target files

Resolve `$ARGUMENTS` to a list of absolute file paths. If the argument is a glob, expand it. If no files are found, report this and stop.

For each file, confirm it imports `Microsoft.Playwright` — skip files that don't.

---

## Phase 1 — Baseline run

Build and run the target test file(s) to confirm the current green state before any changes:

```bash
dotnet build <TestProject.csproj>
dotnet test <TestProject.csproj> --filter "FullyQualifiedName~<TestClassName>"
```

If tests are already red, report the failure and ask the user whether to proceed. Do not refactor a broken test without explicit permission.

Record the baseline test result (pass/fail counts) to `.claude/refactor-work/baseline.txt`.

---

## Phase 2 — Refactor each file

Apply the `playwright-dotnet-refactor` skill to each file in order. For every file:

1. Read the full file content.
2. Identify the user scenario in one sentence from the test name and assertion content.
3. Apply all refactor rules:
   - Upgrade locators to `GetByRole` > `GetByLabel` > `GetByText` > `GetByTestId` > CSS/XPath.
   - Replace weak or missing assertions with user-observable ones.
   - Extract repeated interaction sequences into helpers only when readability clearly improves.
   - Rename tests to `<Behavior>_<ExpectedOutcome>` if the current name is opaque.
4. Write the refactored content back to the same file path.
5. Log a one-line summary of changes made to `.claude/refactor-work/changes.md`.

Do not change the user scenario or remove meaningful test steps.

---

## Phase 3 — Verify green

Re-run the same tests to confirm the refactor did not break anything:

```bash
dotnet test <TestProject.csproj> --filter "FullyQualifiedName~<TestClassName>"
```

If any test is newly red, revert that file and add it to a "needs manual review" list. Report the list at the end.

---

## Phase 4 — Report

Print a summary to the terminal:

- Files refactored (count).
- Key improvements made per file (locator upgrades, assertion improvements, helper extractions).
- Files skipped (not Playwright, already clean, or reverted due to new failures).
- Any uncertainty or assumptions documented in `.claude/refactor-work/changes.md`.

Do not commit or open a PR unless the user explicitly asks.

</process>
