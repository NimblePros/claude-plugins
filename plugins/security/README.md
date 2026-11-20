# security

Time-boxed, whole-codebase security audits that end in actionable GitHub issues: an epic labeled
`security`, with one sub-issue per confirmed vulnerability labeled `security` and
`severity:critical|high|medium|low`. Stack-neutral — the reviewer learns each project's central
protections before reporting anything as missing.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install security@nimblepros
```

## Commands

| Command | What it does |
|---|---|
| `/security:audit [--minutes N] [--max N] [--dry-run] [--areas authz,data,web]` | Fetches the latest default branch, maps the project's central protections, runs one `security-reviewer` agent per focus area in parallel, verifies every finding against the cited code, then files the epic and sub-issues. Defaults: 5 minutes, 20 findings. |

This complements the built-in `/security-review`, which reviews only a branch's pending changes.

## Agents

| Agent | Purpose |
|---|---|
| `security-reviewer` | Read-only reviewer for one focus area — **authz** (authn/authz, tenant isolation, tokens, sessions), **data** (injection, uploads, SSRF, secrets, CI supply chain) or **web** (XSS, CSRF, redirects, headers, client storage). Returns confirmed findings as JSON, plus what it found sound and what it didn't get to. |

## Scripts

| Script | Purpose |
|---|---|
| `scripts/file-security-issues.mjs` | Files the findings deterministically: creates the labels if missing, files sub-issues most severe first, then the epic with a checklist, and links each as a GitHub sub-issue. Supports `--dry-run` and `--repo owner/name`, and rejects an invalid severity or a missing field. |

## Requirements

- `git`
- `gh` (GitHub CLI, authenticated — run `gh auth login`), with permission to create labels and
  issues in the target repository
- Node.js 18+ — for `scripts/file-security-issues.mjs`
