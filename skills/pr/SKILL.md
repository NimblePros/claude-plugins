---
name: pr
description: Full lifecycle for creating a pull request, from investigation through deployment verification. Use when the user says "create PR", "make PR", "open PR", "ship this", or invokes /pr.
---

# Create PR

Full lifecycle for taking changes from a branch through to a deployed, verified PR.

## Phase 1: Gather Context

Use the `AskUserQuestion` tool to confirm:

**Target branch** (default: `main`)

If the branch name contains a work item ID, extract it automatically. If $ARGUMENTS contains a work item ID, prefer that.

```bash
git branch --show-current
git log main..HEAD --oneline
git diff -w main...HEAD --stat
```

Read the diff to understand what changed. Check for work item IDs in commit messages.

## Phase 2: Validate

Merge latest main before creating the PR:

```bash
git fetch origin main
git merge origin/main
```

<CRITICAL>
Always use merge, never rebase. Rebase breaks build tag traceability.
</CRITICAL>

## Phase 3: Create the PR

See [references/pr-template.md](references/pr-template.md) for the full PR body template.

```bash
gh pr create \
  --title "type(scope): subject" \
  --body "$(cat <<'EOF'
<pr-body-from-template>
EOF
)"
```

The PR title must follow conventional commit format with a hard limit of 65 characters.

## Phase 4: Verify Deployment

After creation, use the `gh` CLI to confirm the PR exists:

```bash
gh pr view --web
```

## Rules

- Keep the description factual
- Never force-push or push to main/master without explicit confirmation
- Be succinct; prefer bullet points to long paragraphs
- Always link to work items when possible
