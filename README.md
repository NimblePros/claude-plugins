# NimblePros Claude Code Plugins

A collection of plug-ins authored, curated, and/or edited by the NimblePros team.

---

## Command add-coverage

A single command that finds the highest-risk untested method, writes a mutation-validated xUnit test, reviews it for quality, commits, and opens a PR.

## What it does

Running `/nimblepros:add-coverage` (or `/add-coverage` when using the standalone install) triggers a bootstrap check followed by a six-phase pipeline:

| Phase | Agent | Output |
|---|---|---|
| 0. Ensure environment | _(orchestrator)_ | Verifies/creates `coverage.runsettings`, `.config/dotnet-tools.json` entries, and required test-project NuGet packages — **only with your approval** |
| 1. Find the gap | `coverage-gap-finder` | `gap-report.md` — highest-CRAP untested method |
| 2. Write the test | `test-author` | New xUnit test file (unit, integration, or Playwright e2e — the cheapest type that can observe the behaviour), passing green |
| 3. Validate it has teeth | `mutation-validator` | `mutation-result.md` — PASS/FAIL via Stryker or manual mutation |
| 4. Review quality | `test-reviewer` | `review-notes.md` — LGTM or numbered refactor requests |
| 5. Commit | _(orchestrator)_ | Branch `test/add-coverage-{ClassName}` pushed |
| 6. Open PR | _(orchestrator)_ | PR with structured description |

The pipeline stops and reports to you if the build is broken, the test fails to validate, or the reviewer flags unresolved issues after one refactor pass. Phase 0 never creates or installs anything silently — it always explains what's missing and asks first.

---

## Requirements

Phase 0 of `/add-coverage` checks all of the following automatically and, where possible, offers to set up what's missing (with your approval before anything is written or installed). This section describes what it's checking for.

**CLI tools** (must be on PATH — Phase 0 verifies but cannot install these for you):

- `dotnet` (.NET 8+)
- `git`
- `gh` (GitHub CLI, authenticated)

**Required .NET local tools** (Phase 0 offers to add these to `.config/dotnet-tools.json` if missing):

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "dotnet-reportgenerator-globaltool": {
      "version": "5.4.5",
      "commands": ["reportgenerator"]
    }
  }
}
```

**Optional .NET local tools** (enables Stryker mutation testing; falls back to manual mutation if absent):

```json
{
  "dotnet-stryker": {
    "version": "4.6.0",
    "commands": ["dotnet-stryker"]
  }
}
```

**Required NuGet packages** in test projects (Phase 0 checks each test project and offers to `dotnet add package` anything missing):

- `xunit`
- `Shouldly`
- `NSubstitute`
- `coverlet.collector`

**Optional NuGet package** — `Microsoft.Playwright`, only needed if a coverage gap warrants an end-to-end browser test. If the `test-author` agent determines an e2e test is the right fit and no Playwright test project exists, the pipeline pauses and asks before setting one up.

**Required repo file** — `coverage.runsettings` at the repo root (Phase 0 offers to create it). See the `crap-analysis` skill for the recommended content.

---

## Installation

### Option A — Claude Code plugin install (recommended)

Add the NimblePros marketplace, then install the plugin:

```bash
claude plugin marketplace add NimblePros/claude-plugin
claude plugin install nimblepros@nimblepros
```

This installs the command, agents, and skills into your global Claude Code configuration so they are available in every project.

### Option B — Per-project install via `.claude/settings.json`

Add the marketplace and plugin to your project's `.claude/settings.json` so all collaborators get it when they trust the folder:

```json
{
  "extraKnownMarketplaces": {
    "nimblepros": {
      "source": {
        "source": "github",
        "repo": "NimblePros/claude-plugin"
      }
    }
  },
  "enabledPlugins": {
    "nimblepros@nimblepros": true
  }
}
```

### Option C — Manual copy (standalone, no marketplace)

Copy the plugin files from this repo into your project's `.claude/` directory to use them as standalone configuration (skills are available without namespacing, e.g. `/add-coverage` instead of `/nimblepros:add-coverage`):

```bash
# From your project root
gh repo clone NimblePros/claude-plugin /tmp/nimblepros-plugin
mkdir -p .claude
cp -r /tmp/nimblepros-plugin/commands .claude/
cp -r /tmp/nimblepros-plugin/agents .claude/
cp -r /tmp/nimblepros-plugin/skills .claude/
```

Commit the `.claude/` directory to your repo so the whole team gets it.

---

## Usage

When installed as a plugin, skills are namespaced by the plugin name:

```bash
/nimblepros:add-coverage [optional: namespace or class name]
```

When installed via Option C (standalone copy into `.claude/`), use the short form:

```bash
/add-coverage [optional: namespace or class name]
```

**Target the whole solution:**

```bash
/nimblepros:add-coverage
```

**Target a specific namespace:**

```bash
/nimblepros:add-coverage NimblePros.App.Api
```

**Target a specific class:**

```bash
/nimblepros:add-coverage CatalogService
```

---

## Project-specific customization

### Test type selection

The `test-author` agent picks the cheapest test type that can actually observe the behaviour under test, per the `test-type-selection` skill:

1. **Unit** — no dependency on out-of-process logic; everything runs in-process (including Blazor components via bunit).
2. **Integration** — the behaviour only exists at a process/infrastructure boundary (real database, HTTP pipeline, file system) or across many collaborating services.
3. **End-to-end** — the behaviour only exists in a real browser driven by user interactions; automated with Playwright.

Most coverage gaps resolve to unit tests, since CRAP scoring targets complex in-process logic. The agent never drops down a level for convenience — only when the behaviour is unobservable at the current one.

### Test placement

There is nothing to configure. The `test-author` agent determines where each new test belongs at runtime, in order of preference:

1. Alongside existing tests for the target class, if any exist.
2. In the test project whose `.csproj` has a `<ProjectReference>` to the source project.
3. By naming convention (`src/YourApp.Api/` → `tests/YourApp.Api.Tests/`) when multiple test projects qualify.

If placement is still ambiguous after those rules, the pipeline pauses and asks you to pick from the candidates rather than guessing.

### CRAP score thresholds

The `coverage-gap-finder` agent targets methods with CRAP > 30 or 0% coverage with cyclomatic complexity ≥ 5. These thresholds are defined in `agents/coverage-gap-finder.md` and can be adjusted by forking this repo.

### Test framework conventions

The `test-authoring` skill enforces xUnit + Shouldly + NSubstitute. If your project uses different libraries (e.g., NUnit, FluentAssertions, Moq), fork this repo and update `skills/test-authoring/SKILL.md` to reflect your actual stack.

---

## How the handoff works

Agents communicate through files written to `.claude/add-coverage-work/` during a run. This directory is created at the start and deleted after the PR is opened. The files are:

| File | Written by | Read by |
|---|---|---|
| `gap-report.md` | `coverage-gap-finder` | `test-author`, `mutation-validator` |
| `new-test-path.txt` | `test-author` | `mutation-validator`, `test-reviewer` |
| `mutation-result.md` | `mutation-validator` | orchestrator (PR body) |
| `review-notes.md` | `test-reviewer` | `test-author` (refactor pass) |

---

## Files generated in your repo

Unlike the handoff files above, these are meant to persist and should be committed:

| File | Created by | Purpose |
|---|---|---|
| `coverage.runsettings` | Phase 0 (with your approval) | OpenCover-format coverage collection config |
| `.config/dotnet-tools.json` | Phase 0 (with your approval) | Pins `dotnet-reportgenerator-globaltool` and, optionally, `dotnet-stryker` |

None of these are created without asking first, and Phase 0 never overwrites a version of these files that already exists.

---

## Troubleshooting

**"No meaningful gap was found"**
All methods have CRAP < 30 and coverage > 80%. Pass a specific namespace to narrow the search, or the codebase genuinely has good coverage — that's a win.

**Mutation validator FAIL**
The test doesn't assert the right observable outcome. The validator reports which mutation survived and what assertion would catch it. Fix the test manually or ask Claude to retry.

**Build broken at Phase 1**
The `coverage-gap-finder` stops immediately if `dotnet build` fails. Fix the build first.

**`gh` not authenticated**
Run `gh auth login` before using `/add-coverage`.

**Stryker not found**
The `mutation-validator` falls back to a single manual mutation. To use Stryker, add `dotnet-stryker` to `.config/dotnet-tools.json` and run `dotnet tool restore`.

---

## Contents

```text
.
├── .claude-plugin/
│   ├── plugin.json                           # Plugin manifest
│   └── marketplace.json                      # Marketplace catalog (enables plugin install)
├── README.md                                 # This file
├── commands/
│   └── add-coverage.md                       # /add-coverage:add-coverage slash command
├── agents/
│   ├── coverage-gap-finder.md                # Phase 1: find the gap
│   ├── test-author.md                        # Phase 2: write the test
│   ├── mutation-validator.md                 # Phase 3: validate it has teeth
│   └── test-reviewer.md                      # Phase 4: review quality
└── skills/
    ├── crap-analysis/
    │   └── SKILL.md                          # CRAP scoring, runsettings, ReportGenerator
    ├── test-authoring/
    │   └── SKILL.md                          # xUnit conventions for this project
    ├── test-type-selection/
    │   └── SKILL.md                          # Unit vs. integration vs. e2e (Playwright) decision ladder
    └── stryker-dotnet/
        └── SKILL.md                          # Stryker.NET invocation and result parsing
```
