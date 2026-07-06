# NimblePros Claude Code Plugin

This plugin currently contains a command for adding coverage to complex methods within dotnet projects. Other skills, commands, and agents may be added in the future.

---

## Command add-coverage

A single command that finds the highest-risk untested method, writes a mutation-validated xUnit test, reviews it for quality, commits, and opens a PR.

## What it does

Running `/nimblepros:add-coverage` (or `/add-coverage` when using the standalone install) triggers a six-phase pipeline:

| Phase | Agent | Output |
|---|---|---|
| 1. Find the gap | `coverage-gap-finder` | `gap-report.md` — highest-CRAP untested method |
| 2. Write the test | `test-author` | New xUnit test file, passing green |
| 3. Validate it has teeth | `mutation-validator` | `mutation-result.md` — PASS/FAIL via Stryker or manual mutation |
| 4. Review quality | `test-reviewer` | `review-notes.md` — LGTM or numbered refactor requests |
| 5. Commit | _(orchestrator)_ | Branch `test/add-coverage-{ClassName}` pushed |
| 6. Open PR | _(orchestrator)_ | PR with structured description |

The pipeline stops and reports to you if the build is broken, the test fails to validate, or the reviewer flags unresolved issues after one refactor pass.

---

## Requirements

**CLI tools** (must be on PATH):

- `dotnet` (.NET 8+)
- `git`
- `gh` (GitHub CLI, authenticated)

**Required .NET local tools** (add to `.config/dotnet-tools.json`):

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

**Required NuGet packages** in test projects:

- `xunit`
- `Shouldly`
- `NSubstitute`
- `coverlet.collector`

**Required repo file** — `coverage.runsettings` at the repo root. See the `crap-analysis` skill for the recommended content.

---

## Installation

### Option A — Claude Code plugin install (recommended)

Add the NimblePros marketplace, then install the plugin:

```bash
claude plugin marketplace add NimblePros/claude-plugin
claude plugin install claude-plugin@nimblepros
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
    "add-coverage@nimblepros": true
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

### Test project mapping

The `test-authoring` skill contains a table mapping source projects to test projects. **Update this table to match your solution structure** before using the plugin.

After installing, the files live in Claude Code's plugin cache. To customize, fork this repo, edit the skill files, and point your marketplace at your fork.

For **standalone installs (Option C)**, open `.claude/skills/test-authoring/SKILL.md` in your project and find the section `## Where to put new test files`. Replace the example rows with your project's actual mapping:

```markdown
| Source project | Test project |
|---|---|
| `src/YourApp.Core/` | `tests/YourApp.Core.Tests/` |
| `src/YourApp.Api/`  | `tests/YourApp.Api.Tests/`  |
```

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

```
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
    └── stryker-dotnet/
        └── SKILL.md                          # Stryker.NET invocation and result parsing
```
