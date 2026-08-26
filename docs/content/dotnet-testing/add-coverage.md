---
title: "/add-coverage command"
description: "Find a coverage gap, write a test, validate it catches regressions, review it, commit, and open a PR"
summary: "The six-phase pipeline that orchestrates the plugin's four agents"
date: 2026-08-13
weight: 10
---

> Find a coverage gap, write a test, validate it catches regressions, review it, commit, and
> open a PR.

```bash
/dotnet-testing:add-coverage                       # target the whole solution
/dotnet-testing:add-coverage Acme.Store.Api        # target a namespace
/dotnet-testing:add-coverage PaymentService        # target a class
```

## The pipeline

| Phase | Runs as | Output |
|---|---|---|
| 0. Ensure environment | orchestrator | Verifies CLI tools, local tools, `coverage.runsettings`, and test-project packages — offers to fix gaps, **only with your approval** |
| 1. Find the gap | `coverage-gap-finder` agent | `gap-report.md` — exactly one highest-CRAP untested method |
| 2. Write the test | `test-author` agent | One new xUnit test file, passing green |
| 3. Validate it has teeth | `mutation-validator` agent | `mutation-result.md` — PASS/FAIL via Stryker or a manual mutation |
| 4. Review quality | `test-reviewer` agent | `review-notes.md` — LGTM or numbered refactor requests |
| 5. Commit | orchestrator | Branch `test/add-coverage-{ClassName}` pushed |
| 6. Open PR | orchestrator | PR describing the gap, the test, and the mutation evidence |

If the reviewer requests changes, `test-author` is re-invoked once as a refactor pass and the
review repeats; if the second review still has issues, the pipeline stops and reports rather than
looping. It also stops (and tells you why) if the build is broken, no meaningful gap exists, or
the mutation validation fails.

## How the agents hand off

Agents share no conversation context. They communicate through files in
`.claude/add-coverage-work/`, created at the start of a run and deleted after the PR opens:

| File | Written by | Read by |
|---|---|---|
| `gap-report.md` | `coverage-gap-finder` | `test-author`, `mutation-validator` |
| `new-test-path.txt` | `test-author` | `mutation-validator`, `test-reviewer` |
| `mutation-result.md` | `mutation-validator` | orchestrator (PR body) |
| `review-notes.md` | `test-reviewer` | `test-author` (refactor pass) |

The isolation is deliberate: `test-reviewer` receives *only* the test file path, so its review is
not anchored by the pipeline's reasoning.

Two files are meant to persist in your repo (created only with approval, never overwritten):
`coverage.runsettings` and `.config/dotnet-tools.json`.

## What each phase leans on

Phase 1 applies the [crap-analysis]({{< ref "crap-analysis" >}}) skill's scoring and exclusion
rules. Phase 2 picks a test type per [test-type-selection]({{< ref "test-type-selection" >}}) and
follows [test-authoring]({{< ref "test-authoring" >}}) conventions exactly. Phase 3 follows
[stryker-dotnet]({{< ref "stryker-dotnet" >}}), falling back to one manual mutation when Stryker
isn't installed. Phase 4 reviews against the same two convention skills Phase 2 wrote under.

## Troubleshooting

- **"No meaningful gap was found"** — all methods have CRAP < 30 and coverage > 80%. Narrow with
  a namespace argument, or celebrate.
- **Mutation validator FAIL** — the test doesn't assert the right observable outcome; the report
  names the surviving mutation and what assertion would catch it.
- **Build broken** — Phase 1 stops immediately if `dotnet build` fails. Fix the build first.
- **`gh` not authenticated** — run `gh auth login`; the pipeline can still run through commit
  without it.
- **Stryker not found** — the validator falls back to a manual mutation; add `dotnet-stryker` to
  `.config/dotnet-tools.json` to use Stryker.
