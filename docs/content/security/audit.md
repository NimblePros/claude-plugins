---
title: "/audit command"
description: "Time-boxed security review of the default branch, filed as a GitHub epic with severity-labeled sub-issues"
summary: "Parallel reviewers, verified findings, one issue per vulnerability"
date: 2026-11-20
weight: 10
---

> Time-boxed security review of the default branch; files an epic with one sub-issue per
> vulnerability, labeled security and severity.

```bash
/security:audit                                  # 5 minutes, at most 20 findings, all areas
/security:audit --minutes 15 --max 30
/security:audit --areas authz,web --dry-run      # print the issues instead of filing them
```

| Argument | Default | Meaning |
|---|---|---|
| `--minutes N` | `5` | Time box for the review |
| `--max N` | `20` | At most N findings; the review stops at whichever limit comes first |
| `--areas` | `authz,data,web` | Focus areas, one `security-reviewer` agent each |
| `--dry-run` | off | Review and print the issues that would be filed, without filing them |

## The pipeline

| Step | Runs as | Output |
|---|---|---|
| 1. Get latest | orchestrator | `origin/<default-branch>` checked out (fast-forward or worktree), branch and SHA noted |
| 2. Map the protections | orchestrator | A short context note: stack, where auth, policies, antiforgery, headers, rate limits and tenant resolution live, and any ADRs accepting a risk |
| 3. Review in parallel | one `security-reviewer` per area | Confirmed findings as JSON, plus "checked and found sound" and "not reviewed" lists |
| 4. Verify | orchestrator | Every cited `file:line` re-read; wrong findings dropped or re-rated, duplicates merged, cut to `--max` |
| 5. Write `findings.json` | orchestrator | The input for the filing script, in a temp directory |
| 6. File the issues | `scripts/file-security-issues.mjs` | Labels created if missing, sub-issues filed most severe first, then the epic, each linked as a sub-issue |
| 7. Report | orchestrator | Epic link, count per severity, and each sub-issue's number, severity and title |

The verification step exists because reviewer agents can be confidently wrong. The command never
fixes anything: each sub-issue is meant to ship as its own PR.

## Severity rubric

| Severity | Meaning |
|---|---|
| `critical` | Remote, unauthenticated or cross-tenant compromise of data or accounts |
| `high` | An authenticated user reads or changes data they shouldn't, or privilege escalation |
| `medium` | Needs an unusual precondition (supply chain, misconfiguration, user interaction) but the impact is serious |
| `low` | Limited impact, nuisance, or defense in depth with a concrete gap |

## The filing script

`scripts/file-security-issues.mjs` makes the GitHub side deterministic, and can be run by hand:

```bash
node scripts/file-security-issues.mjs findings.json [--dry-run] [--repo owner/name]
```

It validates every finding (a valid severity, plus `title`, `where`, `problem`, `exploit` and
`fix`) before touching GitHub. The `findings.json` format is documented at the top of the script.
If the review finds nothing, the epic is still filed, because it records what was checked.
