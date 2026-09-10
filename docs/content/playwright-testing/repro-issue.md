---
title: "/repro-issue command"
description: "Reproduce a GitHub issue with Playwright, capture evidence artifacts, and post a structured comment"
summary: "From issue number to posted evidence comment in four automated phases"
date: 2026-08-14
weight: 30
aliases: ["/testing/repro-issue/"]
---

> Reproduce a GitHub issue with Playwright, collect screenshot/video/trace artifacts, and publish
> evidence-based issue comments.

```bash
/playwright-testing:repro-issue 42                              # by issue number
/playwright-testing:repro-issue https://github.com/org/repo/issues/42
```

## The pipeline

| Phase | What happens |
|---|---|
| 0. Prerequisites | Verifies `dotnet`, `git`, `gh auth`; installs Playwright Chromium if needed |
| 1. Fetch issue | `gh issue view` — extracts expected behaviour, observed behaviour, and preconditions |
| 2. Build repro | Drafts or adapts a Playwright C# test using semantic locators and user-visible assertions |
| 3. Run and collect | Runs a targeted test filter; collects `failure.png`, `trace.zip`, and video from `TestResults/PlaywrightArtifacts/` |
| 4. Post comment | Populates and posts the issue comment template; falls back to printing if `gh auth` is unavailable |

## Repro status

The command always declares one of three outcomes — it never posts ambiguous commentary:

- **Reproduced** — test failed with the same observable symptom as the issue.
- **Not Reproduced** — test passed; what was checked is documented.
- **Inconclusive** — test errored for unrelated reasons or environment could not be verified.

## What it leans on

Built on the [`playwright-issue-repro-report`]({{< ref "playwright-issue-repro-report" >}}) skill.
The comment it posts follows `templates/issue-comment-template.md` from that skill; the Playwright
test conventions follow the same semantic-locator priority as the broader testing practice.

## Handoff files

| File | Written by |
|---|---|
| `.claude/repro-work/issue-context.md` | Phase 1 — issue title, expected/observed, preconditions |
| `.claude/repro-work/repro-test-path.txt` | Phase 2 — path to the repro test file |
| `.claude/repro-work/artifacts.md` | Phase 3 — paths to screenshot, trace, and video |
| `.claude/repro-work/issue-comment.md` | Phase 4 — ready-to-post comment body |

The `.claude/repro-work/` directory is intentionally left after the run as an audit trail.
