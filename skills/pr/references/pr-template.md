# PR Body Template

```markdown
## Description

{2-4 sentences: what changed, why, and any notable technical decisions}

## Related Issue(s)

- #{issueNumber}

## Checklist

- [ ] The PR includes tests for the changes (if applicable).
- [ ] The documentation has been updated (if applicable).

## Additional Notes for Reviewers

{Non-obvious impacts, deployment notes, areas to focus review on. Otherwise "None."}
```

## PR Title

Must follow conventional commit format.

```text
type(scope): subject
```

Types: `fix`, `feat`, `refactor`, `chore`, `docs`, `test`, `perf`

## gh pr create Command

```bash
gh pr create \
  --title "type(scope): subject" \
  --body "$(cat <<'EOF'
## Description

{description}

## Related Issue(s)

- #{issueNumber}

## Checklist

- [x] The PR includes tests for the changes (if applicable).
- [x] The documentation has been updated (if applicable).

## Additional Notes for Reviewers

{notes or "None."}
EOF
)"
```
