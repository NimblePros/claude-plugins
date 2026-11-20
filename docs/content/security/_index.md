---
title: "security"
description: "Time-boxed whole-codebase security audits: parallel focus-area reviewers, verified findings, and a GitHub epic with one severity-labeled sub-issue per vulnerability"
summary: "Audit a codebase and file the findings as GitHub issues"
date: 2026-11-20
weight: 90
---

Time-boxed, whole-codebase security audits that end in actionable GitHub issues. Stack-neutral:
the reviewer learns each project's central protections before it reports anything as missing.

```bash
claude plugin install security@nimblepros
```

## Commands

| Command | One-line summary |
|---|---|
| [`/security:audit`]({{< ref "audit" >}}) | Review the latest default branch in parallel by focus area, verify every finding, and file an epic with one severity-labeled sub-issue per vulnerability |

The built-in `/security-review` covers a different case: it reviews only the pending changes on a
branch. Use `/security:audit` for a periodic look at the whole codebase.

## Agents

**`security-reviewer`** is a read-only reviewer for one focus area: **authz** (authentication,
authorization, tenant isolation, tokens and sessions), **data** (injection, uploads, SSRF,
secrets, CI supply chain) or **web** (XSS, CSRF, open redirects, headers, client-side storage).
It reports only findings it confirmed in the code, each with `file:line`, severity, a concrete
exploit and a fix, as JSON the filing script accepts. It also lists what it checked and found
sound, and what it ran out of time for.

## Requirements

- `git`
- `gh` (GitHub CLI, authenticated — run `gh auth login`), allowed to create labels and issues
- Node.js 18+ for the issue-filing script
