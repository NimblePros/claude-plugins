# Creating approved sub-issues

Only follow this after the user has approved a split proposed in [triage.md](triage.md).
Create the sub-issues exactly as approved, in the approved order.

## 1. Create each sub-issue

```bash
gh issue create --title "<title>" --body "<scope and acceptance criteria>

Part of #<parent>"
```

Copy the parent's milestone and area labels with `--milestone` and `--label` when they
apply. `gh issue create` prints the new issue's URL; the number is at the end of it.

## 2. Link it to the parent as a native sub-issue

The sub-issues API needs the issue's internal **id**, not its number:

```bash
id=$(gh api repos/{owner}/{repo}/issues/<child-number> --jq .id)
gh api -X POST repos/{owner}/{repo}/issues/<parent>/sub_issues -F sub_issue_id="$id"
```

Link them in the approved order, because that order sets the sub-issue priority on GitHub.

## 3. Record dependencies (optional)

If one sub-issue must wait for another, add "Blocked by #NNN" to its body. That way a later
triage can see the dependency.

## 4. Report and continue

List the created issues with their URLs, then restart at step 1 ([context.md](context.md))
with the first sub-issue.
