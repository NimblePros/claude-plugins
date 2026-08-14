---
description: Run Stryker.NET mutation testing against a source file or class and report survived mutations.
argument-hint: <source-file-path-or-class-name>
allowed-tools: Bash(dotnet *), Bash(pwsh *), Write, Read, Glob
---

<objective>
Execute the `stryker-dotnet` skill against the source file or class identified by `$ARGUMENTS`: locate the owning test project, run Stryker scoped to the target, parse the JSON report, and output a summary of killed vs. survived mutations with actionable recommendations.

`$ARGUMENTS` must be a source file path (e.g. `src/MyApp/Services/OrderService.cs`) or a class name (e.g. `OrderService`). If omitted, print usage and stop.
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
If `$ARGUMENTS` is empty, print usage and stop.

### 0.2 Resolve the target file

If `$ARGUMENTS` looks like a class name (no path separators), locate the file:
```bash
Get-ChildItem -Recurse -Filter "*.cs" | Select-String -Pattern "class $ARGUMENTS\b"
```

If multiple files match, list them and ask the user to choose one.

### 0.3 Stryker availability

```powershell
$local  = dotnet tool list 2>&1
$global = dotnet tool list -g 2>&1
$available = ($local -match "dotnet-stryker") -or ($global -match "dotnet-stryker")
```

If not available, ask permission to add `dotnet-stryker` to `.config/dotnet-tools.json` — follow the `stryker-dotnet` skill for the exact JSON entry. If approved:
```bash
dotnet tool restore
```

If the user declines, stop — mutation testing requires Stryker.

---

## Phase 1 — Locate the test project

Glob for test projects (`**/*.Tests*.csproj`). Inspect `<ProjectReference>` entries to find the project that contains the target file. If multiple test projects qualify, prefer the one whose name contains `Unit`. Ask the user to confirm if still ambiguous.

---

## Phase 2 — Run Stryker

Follow the `stryker-dotnet` skill to run mutation testing scoped to the target file:

```powershell
Push-Location "<TestProjectDir>"
dotnet stryker `
  --mutate "**/<TargetFile>.cs" `
  --reporter json `
  --reporter progress `
  --output "../../stryker-output"
Pop-Location
```

---

## Phase 3 — Parse and report

Read `stryker-output/reports/mutation-report.json`. Following the `stryker-dotnet` skill, extract:
- Total mutations, killed, survived, mutation score (%)

Present a summary table of **survived mutations** only (these are the test gaps):

| Line | Mutator | Original → Mutated | Status |
|------|---------|--------------------|--------|

If mutation score ≥ 80%: report success and note survivors as improvement opportunities.
If below 80%: identify the highest-impact survivors as specific test gaps to address next.

</process>
