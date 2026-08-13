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

- [x] Read the existing ADR template and follow its section names/ordering
- [x] Write `devdocs/ADRs/0001-2026-08-13-multi-plugin-marketplace-structure.md` covering:
      marketplace-of-plugins model, language-specific vs. language-agnostic split, skills/agents/
      commands grouped by capability, progressive disclosure, alternatives, consequences
- [x] Add `devdocs/ADRs/README.md` index listing ADRs

## Phase 2 — Folder structure

- [x] Create `plugins/<name>/` for the five plugins above
- [x] `git mv` every existing skill, agent, and command into its plugin
- [x] Add `plugins/<name>/.claude-plugin/plugin.json` for each plugin
- [x] Add a short `plugins/<name>/README.md` for each plugin
- [x] Rewrite root `.claude-plugin/marketplace.json` to list all five plugins with
      `./plugins/<name>` sources
- [x] Delete the root `.claude-plugin/plugin.json` (no plugin is rooted at `.` any more)
- [x] Confirm root `skills/`, `agents/`, `commands/` directories are gone

## Phase 3 — Documentation

- [x] Rewrite root `README.md` as a marketplace overview: plugin table, per-plugin install
      commands, updated namespaced command names (`/dotnet-testing:add-coverage`), corrected
      repo URLs (`claude-plugin` → `claude-plugins`), updated contents tree
- [x] Move the add-coverage detail out of the root README into
      `plugins/dotnet-testing/README.md`

## Phase 4 — Automated verification

This repository contains no compiled code, so `dotnet BuildTestFormat.cs` does not apply. The
equivalent mechanical checks, run via `python tests/validate_marketplace.py` plus
`claude plugin validate .`:

- [x] Every `*.json` manifest parses; `claude plugin validate .` passes against the real schema
- [x] Every `marketplace.json` `source` path starts with `./`, exists, and contains
      `.claude-plugin/plugin.json` whose `name` matches the catalog entry
- [x] Marketplace has `name`, `owner.name`, and a `plugins` array; no duplicate plugin names;
      the `renames` map resolves
- [x] Every moved `SKILL.md`/agent/command still has valid YAML frontmatter with a `name`
      (agents/skills) or `description` (commands), and each name matches its location
- [x] No plugin is empty, and no agent/command/skill references a skill that ships in a
      different plugin (cross-plugin dependency = broken install)
- [x] Nothing left at the repo root (`skills/`, `agents/`, `commands/`, root `plugin.json`)
- [x] Nothing lost in the move — verified once by hand with `git ls-tree main` vs `HEAD`:
      19 skills → 19, 11 agents → 11, 1 command → 1. This is a migration check, not an ongoing
      invariant, so it is deliberately *not* in the validator (a fixed expected count would have
      to be edited on every legitimate addition).
- [x] Each new check verified adversarially: broken in a scratch copy of the repo, confirmed to
      exit 1, restored
- [x] Record the check script output in the PR body

## Phase 5 — Independent review

- [x] Launch a separate review agent (fresh context) against the full diff and the issue's
      acceptance criteria
- [x] Address all significant findings; record any deliberately skipped ones below
- [x] Re-run Phase 4 checks after the review changes

## Phase 6 — PR

- [x] Commit in coherent chunks, push branch
- [x] `gh pr create` with summary, verification results, and `Fixes #2` —
      https://github.com/NimblePros/claude-plugins/pull/3

## Review findings

The review agent raised 15 findings. Its verification also confirmed, independently, that all
five manifests pass `claude plugin validate`, that nothing was lost in the move, and that no
agent or command loads a skill from another plugin.

Addressed:

1. **No `renames` entry for the removed `nimblepros` plugin** (significant). Confirmed against the
   marketplace docs, then added `"renames": { "nimblepros": null }` so existing installs migrate
   with a notice instead of `plugin-not-found`. README and ADR updated to match.
2. **`feature-specification` named agents from other plugins** (`dotnet-agent`) and agents that do
   not exist anywhere (`react-agent`, `test-agent`) — the one real counterexample to the
   "no cross-plugin dependencies" claim. Replaced with role placeholders.
3. **Validator accepted a `source` without the required `./` prefix.** Added the prefix check.
4. **Validator computed component counts but never asserted anything with them**, so moving a
   skill into the wrong plugin passed. Replaced with two checks that hold as ongoing invariants:
   no empty plugin, and no cross-plugin skill reference. (See the Phase 4 note on why a fixed
   expected count is not the right check.)
5. **Validator ignored missing marketplace `name`/`owner`.** Now required.
6. **Duplicate plugin names were silently absorbed by a `set`.** Now an error.
7. **"Checked N plugins…" printed to stdout even on failure**, so a stdout-only CI log read as a
   pass. The summary now prints only when there are no errors.
8. **ADR listed the validation script as future work** although it shipped in the same change.
   Moved to the Consequences mitigation; only "wire it into CI" remains as future work.
9. **ADR cited a PR checklist that does not exist.** Now points at `tests/validate_marketplace.py`.
10. **ADR arithmetic** — "twelve files" for 1 command + 4 agents + 4 skills. Now "nine".
11. **README layout tree listed a `docs/` directory** that is not in the repository. Removed, and
    the empty local directory deleted; `tests/` added to the tree instead.
14. **Plugin READMEs used relative sibling links** (`../dotnet-testing/`) that are dead once a
    plugin is installed on its own. Now absolute GitHub URLs.
15. **Two disagreeing ADR conventions now ship in one repo** (this repo's plain-heading style vs.
    the `adr-writing` template's Hugo frontmatter style). `devdocs/ADRs/README.md` now states
    which one applies here and why.

Deliberately deferred, with reasons:

- **12 — `technical-writer` hard-codes a Hugo/Geekdoc site and paths from another repository**
  and **13 — `publish-nuget` hard-codes `Nimble.Endpoints`.** Both files moved unchanged (0 diff
  lines); rewriting agent bodies is content work, not the structural change this issue asks for.
  Mitigated now: the misleading Hugo claim was removed from `technical-writer`'s frontmatter
  description (which loads into every session), and `plugins/dotnet/README.md` states the
  `publish-nuget` limitation. Full genericization is recorded in the ADR's Future Considerations.
- **The `$schema` URL in `marketplace.json` 302-redirects.** Pre-existing, unchanged from `main`,
  and Claude Code ignores `$schema` at load time. The reviewer classified it as "not a finding".
