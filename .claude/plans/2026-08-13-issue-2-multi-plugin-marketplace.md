# Issue #2 — Create a First Pass at Multiple Plugins in Marketplace

- Issue: [#2](https://github.com/NimblePros/claude-plugins/issues/2)
- Branch: `issue-2-multi-plugin-marketplace`
- Date: 2026-08-13

## Goal

Convert this repository from a single root-level plugin (`nimblepros@nimblepros`, `source: "."`)
into a marketplace that catalogs several focused plugins under `plugins/`, and record the
reasoning in an ADR under `devdocs/`.

## Assumptions

The issue's sample tree lists six aspirational plugins (`dotnet`, `architecture`, `testing`,
`frontend`, `databases`, `devops`) and skills that do not exist in this repo yet. Empty plugin
directories are not installable and would advertise capabilities we do not have, so:

1. Only plugins with real content are created; the rest are named as future work in the ADR.
2. Existing skills/agents/commands are moved (via `git mv`, preserving history), not rewritten.
3. Splitting oversized `SKILL.md` files via progressive disclosure is recorded as a follow-up —
   the issue asks for *structure* first, and every current `SKILL.md` is already well under the
   ~500-line guidance (largest is `csharp-coding-standards`, which already uses supporting files).
4. The repo's ADR folder is `devdocs/ADRs` (already present, with a template). The issue says
   `/devdocs/adrs`; existing repo convention wins per the `adr-writing` skill.

## Plugin taxonomy (first pass)

| Plugin | Scope | Contents |
|---|---|---|
| `dotnet` | Language/framework-specific .NET practice | skills: csharp-coding-standards, package-management, microsoft-extensions-configuration, microsoft-extensions-dependency-injection, opentelemetry-dotnet-instrumentation; agents: dotnet-agent, publish-nuget |
| `dotnet-testing` | The .NET coverage/mutation testing product | command: add-coverage; agents: coverage-gap-finder, test-author, mutation-validator, test-reviewer; skills: crap-analysis, stryker-dotnet, test-authoring, test-type-selection, aspire-integration-testing, playwright-dotnet-refactor |
| `testing` | Language-agnostic testing | agents: e2e-tester; skills: playwright-issue-repro-report |
| `architecture` | Architecture decisions and technical documentation | skills: adr-writing; agents: technical-writer |
| `workflow` | Issue → spec → implementation → PR loop (gh CLI) | skills: feature-specification, implement-feature, commit, conventional-commit, pr, review; agents: issue-agent, spec-agent, pr-agent |

Cohesion rule applied: a skill ships in the same plugin as the agent/command that loads it, so no
plugin depends on another being installed.

## Phase 1 — ADR

- [ ] Read the existing ADR template and follow its section names/ordering
- [ ] Write `devdocs/ADRs/0001-2026-08-13-multi-plugin-marketplace-structure.md` covering:
      marketplace-of-plugins model, language-specific vs. language-agnostic split, skills/agents/
      commands grouped by capability, progressive disclosure, alternatives, consequences
- [ ] Add `devdocs/ADRs/README.md` index listing ADRs

## Phase 2 — Folder structure

- [ ] Create `plugins/<name>/` for the five plugins above
- [ ] `git mv` every existing skill, agent, and command into its plugin
- [ ] Add `plugins/<name>/.claude-plugin/plugin.json` for each plugin
- [ ] Add a short `plugins/<name>/README.md` for each plugin
- [ ] Rewrite root `.claude-plugin/marketplace.json` to list all five plugins with
      `./plugins/<name>` sources
- [ ] Delete the root `.claude-plugin/plugin.json` (no plugin is rooted at `.` any more)
- [ ] Confirm root `skills/`, `agents/`, `commands/` directories are gone

## Phase 3 — Documentation

- [ ] Rewrite root `README.md` as a marketplace overview: plugin table, per-plugin install
      commands, updated namespaced command names (`/dotnet-testing:add-coverage`), corrected
      repo URLs (`claude-plugin` → `claude-plugins`), updated contents tree
- [ ] Move the add-coverage detail out of the root README into
      `plugins/dotnet-testing/README.md`

## Phase 4 — Automated verification

This repository contains no compiled code, so `dotnet BuildTestFormat.cs` does not apply. The
equivalent mechanical checks are:

- [ ] Every `*.json` manifest parses and matches the marketplace/plugin schema shape
- [ ] Every `marketplace.json` `source` path exists and contains `.claude-plugin/plugin.json`
- [ ] Every moved `SKILL.md`/agent/command still has valid YAML frontmatter with a `name`
      (agents/skills) or `description` (commands)
- [ ] File count of skills/agents/commands before == after (nothing dropped in the move)
- [ ] No file references a path that no longer exists (`grep` for `skills/`, `agents/`,
      `commands/` root-relative references in markdown)
- [ ] Record the check script output in the PR body

## Phase 5 — Independent review

- [ ] Launch a separate review agent (fresh context) against the full diff and the issue's
      acceptance criteria
- [ ] Address all significant findings; record any deliberately skipped ones here
- [ ] Re-run Phase 4 checks if the review changed anything

## Phase 6 — PR

- [ ] Commit in coherent chunks, push branch
- [ ] `gh pr create` with summary, verification results, and `Fixes #2`

## Review findings

_(filled in during Phase 5)_
