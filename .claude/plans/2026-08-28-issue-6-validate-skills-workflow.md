# Issue 6 — GitHub Workflow That Validates Skills

**GitHub Issue**: [#6](https://github.com/NimblePros/claude-plugins/issues/6)
**Status**: Implemented — [PR #8](https://github.com/NimblePros/claude-plugins/pull/8) pending merge

## Overview

Add a GitHub Actions workflow that validates every plugin and skill in the marketplace with
`claude plugin validate` and the reference Agent Skills validator
([`skills-ref`](https://github.com/agentskills/agentskills/tree/main/skills-ref)), so a PR
cannot merge with a manifest or `SKILL.md` that the Claude Code runtime or the portable spec
would reject. The repo's existing structural validator (`scripts/ValidateMarketplace.cs`) runs
in the same job because the README already lists it as a pre-PR check and it catches catalog ↔
directory mismatches neither of the other two tools look at.

## Phases

### Phase 1: Workflow — done

- [x] `.github/workflows/validate.yml` on push to `main`, every PR, and `workflow_dispatch`
- [x] Install Claude Code CLI (`latest`, pinnable via `CLAUDE_CODE_VERSION`) and `skills-ref`
      from a pinned commit (not on PyPI)
- [x] `claude plugin validate . --strict` for the marketplace catalog
- [x] `claude plugin validate plugins/<p> --strict` for each plugin manifest and its
      skill/agent/command frontmatter
- [x] `scripts/validate_skills.py` — walks every `SKILL.md`, runs skills-ref's checks, reads as
      UTF-8, allowlists documented Claude Code frontmatter extensions, emits `::error`
      annotations and a step summary
- [x] `dotnet run scripts/ValidateMarketplace.cs` for repository structure

### Phase 2: Pre-existing failures the workflow surfaced — done

- [x] `marketplace.json` catalogued `testing` at `./plugins/testing` (half-finished rename from
      cfaac80); repointed to `playwright-testing` and added a `renames` entry
- [x] `plugins/architecture/commands/adr.md` `argument-hint` was invalid YAML; quoted
- [x] `opentelemetry-dotnet-instrumentation` carried non-spec `version`/`tags`; moved under
      `metadata`

### Phase 3: Review findings — done

- [x] **`invocable` is not a Claude Code field.** Ten skills used `invocable: true|false`; the
      runtime only knows `user-invocable` and `disable-model-invocation`, so the key was silently
      ignored and the six skills meant as background knowledge were still in the `/` menu. The
      first cut of `validate_skills.py` allowlisted `invocable`, which hid the defect.
      Fix: `invocable: false` → `user-invocable: false`; `invocable: true` dropped (it is the
      default); the allowlist now holds only the fields the Claude Code docs define, so a
      misspelled or invented key fails CI.

### Phase 4: Verification — done

- [x] All four steps pass locally and in CI on the PR branch
- [x] Failure path exercised: a skill with a bare `invocable` key produces the annotation and
      exit 1; a skill using `user-invocable`, `disable-model-invocation`, and `argument-hint`
      passes

## Automated test requirements

This change is CI configuration plus frontmatter; there is no application code to unit test.
The workflow itself is the test — it runs on this PR — and the wrapper script's failure path
was exercised by hand as noted above.

## Deferred / not in scope

- The Hugo docs under `docs/content/docs/` still describe a `testing` plugin; the rename to
  `playwright-testing` there is a docs task separate from validation.
- `CLAUDE_CODE_VERSION` tracks `latest` deliberately so validation keeps pace with the runtime;
  pin it if an upstream release breaks the job.
