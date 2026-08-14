---
name: review
description: Review code changes using parallel agents. Works on local diffs or open PRs. Checks project conventions, scans for bugs, verifies claims, and resolves review threads. Use when the user says "review", "review this", "review PR", "check this PR", or invokes /review.
---

# Review

Multi-agent code review that works on local diffs or open PRs.

## Step 1: Determine the Review Target

If $ARGUMENTS contains a PR number, review that PR:

```bash
gh pr view {number} --json title,body,headRefName,baseRefName,labels,state
gh pr diff {number}
```

If no PR number, review the local diff against main:

```bash
git diff -w origin/main...HEAD
```

<IMPORTANT>
A PR is not required. The review works on any diff. Do not create a PR just to run a review.
</IMPORTANT>

## Step 2: Dispatch Parallel Review Agents

Launch each one as a background agent. Each agent gets the diff and reviews independently.

**Agent: Bug Scan**
Shallow scan for obvious bugs in the changed code. Focus on logic errors, wrong file paths, incorrect commands, broken references. Ignore style issues, linting, and type errors (CI catches those). Only report issues a senior engineer would flag.

**Agent: Git History Context**
Read git blame and history of modified files. Check for conflicts with recent changes, patterns from previous PRs, and whether the changes are consistent with the file's evolution.

**Agent: Shared Code Impact**
For each modified file, check all callers and consumers. Flag if a shared file was changed without considering all code paths that use it. This is the #1 cause of errors that make it into production.

**Agent: Project Convention Check**
Check project-specific rules:
- Branch name follows the project's naming convention (check CONTRIBUTING.md, CI config, or recent branch names for the pattern)
- PR title follows conventional commit format, under 65 chars

**Agent: Security and Secrets Check**

- Scan for hardcoded secrets, credentials, or sensitive information in the diff. Flag any findings with high confidence.
- Check for OWASP Top 10 vulnerabilities in the changed code, such as SQL injection, XSS, or insecure deserialization patterns.
- Ensure that any changes to authentication or authorization logic follow best practices and do not introduce security weaknesses.
- Flag any use of deprecated or insecure APIs that could lead to security issues.
- Run configured static analysis tools for security and report any findings that are relevant to the changed code.

## Step 3: Score and Filter

For each issue found, score confidence 0-100:
- **0-25**: Likely false positive
- **25-50**: Might be real, could not verify
- **50-75**: Verified issue, but minor or unlikely to hit in practice
- **75-100**: Verified issue, will impact functionality or was explicitly called out in CLAUDE.md

<CRITICAL>
Only report issues scoring 75 or above. Filter out everything else. False positives erode trust in the review process.
</CRITICAL>

## Step 4: Produce the Review

```markdown
## Code Review

Found {N} issues:

1. {description} ({evidence source})

   {link to file and line, or quote from diff}

2. ...
```

If no issues score 75+:

```markdown
## Code Review

No issues found. Checked for bugs, shared code impact, project conventions, and security issues.
```

## Step 5: Resolve Existing Threads (PR only)

If reviewing an open PR, check for existing review threads:

```bash
gh api graphql -f query='{
  repository(owner: "{orgName}", name: "{repositoryName}") {
    pullRequest(number: {N}) {
      reviewThreads(first: 50) {
        nodes { id isResolved comments(first: 1) { nodes { body } } }
      }
    }
  }
}'
```

For each unresolved thread, check if the issue has been fixed in the current diff. If fixed, resolve the thread:

```bash
gh api graphql -f query='mutation { resolveReviewThread(input: {threadId: "{id}"}) { thread { isResolved } } }'
```

<IMPORTANT>
Only resolve threads where the fix is verifiable in the diff. Do not resolve threads based on assumptions.
</IMPORTANT>
