---
description: Time-boxed security review of the default branch; files an epic with one sub-issue per vulnerability, labeled security and severity
argument-hint: "[--minutes N] [--max N] [--dry-run] [--areas authz,data,web]"
allowed-tools: Bash(git *), Bash(gh *), Bash(node *), Read, Grep, Glob, Write
---

# Security audit

Review the latest default branch (usually `main`) for security vulnerabilities within a time box, prioritized by
severity, and file the results as GitHub issues: an epic labeled `security`, with one sub-issue per vulnerability
labeled `security` and `severity:critical|high|medium|low`.

(This is a whole-codebase audit. The built-in `/security-review` reviews only the pending changes on a branch.)

Arguments: `$ARGUMENTS`

- `--minutes N` — time box for the review (default **5**).
- `--max N` — at most N vulnerabilities (default **20**). Stop at whichever limit comes first.
- `--areas` — focus areas to run (default `authz,data,web`; see the `security-reviewer` agent).
- `--dry-run` — review and print the issues that would be filed, without filing them.

## Steps

1. **Get latest.** Find the default branch (`gh repo view --json defaultBranchRef -q .defaultBranchRef.name`),
   `git fetch origin`, and review `origin/<default>` (fast-forward the current branch if it's on the default branch,
   behind and clean; otherwise review in a worktree of `origin/<default>`). Note the branch and short commit SHA.
2. **Map the protections.** Spend a minute building a short context note the reviewers share: the stack, where
   authentication, authorization policies, CSRF/antiforgery, security headers, rate limits and tenant resolution
   are wired up, and any ADRs or docs that accept a risk. Read `CLAUDE.md` / `AGENTS.md` first if present. This
   keeps reviewers from reporting centrally handled protections as missing.
3. **Review in parallel.** Start one `security-reviewer` agent per focus area, in a single message so they run
   concurrently, in the foreground. Give each its area, the context note, a time budget a minute under
   `--minutes`, and a finding cap of about `--max` divided by the number of areas (round up). Tell them to report
   only confirmed findings.
4. **Verify.** Agents can be confidently wrong. For every finding, open the cited `file:line` yourself (one batched
   grep/sed is enough) and drop or re-rate anything that doesn't hold. Merge duplicates. Keep the most severe first and
   cut to `--max`.
5. **Write the findings file** to the scratchpad (or a temp directory) as `findings.json`, in the format documented
   at the top of `${CLAUDE_PLUGIN_ROOT}/scripts/file-security-issues.mjs`: title (`Epic: security review YYYY-MM-DD`),
   branch, commit, scope, the reviewers' "checked and found sound" and "not reviewed" lists, and the findings.
6. **File the issues.** Run `node "${CLAUDE_PLUGIN_ROOT}/scripts/file-security-issues.mjs" <path>/findings.json`
   (add `--dry-run` when asked). The script creates the labels if missing, files the sub-issues most severe first,
   then the epic with a checklist, and links each one as a GitHub sub-issue of the epic.
7. **Report** the epic link, the count per severity, and each sub-issue's number, severity and title. If nothing was
   found, still file the epic (it records what was checked) unless `--dry-run`.

Don't fix the vulnerabilities in this command; each sub-issue ships as its own PR.
