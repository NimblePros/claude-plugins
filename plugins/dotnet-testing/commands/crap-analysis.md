---
description: Collect code coverage, generate a CRAP score report, and surface the highest-risk methods.
argument-hint: [project-or-namespace-filter]
allowed-tools: Bash(dotnet *), Write, Read, Glob
---

<objective>
Execute the `crap-analysis` skill: run tests with coverage collection, generate an HTML report with Risk Hotspots, and present the top CRAP-scored methods with recommended actions.

`$ARGUMENTS` optionally scopes the analysis to a specific project name or namespace (e.g. `Acme.Store.Api`). If omitted, analyse all test projects.
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

### 0.2 ReportGenerator

```bash
dotnet tool list
dotnet tool list -g
```

Check whether `dotnet-reportgenerator-globaltool` is available (local or global). If missing, ask permission before adding it to `.config/dotnet-tools.json` — follow the `crap-analysis` skill for the exact JSON entry. If approved:
```bash
dotnet tool restore
```

### 0.3 coverage.runsettings

Check whether `coverage.runsettings` exists in the repository root. If missing, follow the `crap-analysis` skill to create one with `Format=cobertura,opencover` — show the content to the user before writing.

---

## Phase 1 — Run tests with coverage

Glob for test projects (`**/*.Tests*.csproj`, `**/*Tests.csproj`). If `$ARGUMENTS` scopes to a project or namespace, filter to matching projects only.

For each test project:
```bash
dotnet test <TestProject> `
  --settings coverage.runsettings `
  --collect:"XPlat Code Coverage" `
  --results-directory ./TestResults
```

If a project fails, report the failure and continue with the remaining projects so coverage is still collected.

---

## Phase 2 — Generate report

```bash
dotnet reportgenerator `
  -reports:"TestResults/**/coverage.opencover.xml" `
  -targetdir:"coverage" `
  -reporttypes:"Html;TextSummary;MarkdownSummaryGithub"
```

---

## Phase 3 — Extract and present hotspots

Read `coverage/Summary.txt` and the coverage data to extract the top-10 Risk Hotspots by CRAP score.

Present a table:

| Method | Complexity | Coverage | CRAP Score | Priority |
|--------|-----------|---------|-----------|---------|

- CRAP > 100: **critical**
- CRAP 30–100: **high**
- CRAP 5–30: medium

End with a recommendation: which single method has the highest improvement potential (highest CRAP score that would drop significantly with moderate coverage increase).

</process>
