---
description: Merge latest main, create a pull request with a conventional-format title and structured body, and verify deployment.
argument-hint: "[--target <branch>] [--work-item <id>] [--draft]"
allowed-tools: Bash(git *), Bash(gh *)
---

<objective>
Execute the `pr` skill: validate the current branch, merge latest main, create a PR, then verify deployment.

`$ARGUMENTS` may supply a target branch (`--target <branch>`), work item ID (`--work-item <id>`), or `--draft` flag. If omitted, target defaults to `main` and the work item is inferred from the branch name.
</objective>

<context>
Current branch: !`git branch --show-current`
Commits ahead of main: !`git log main..HEAD --oneline`
</context>

<process>
Follow the `pr` skill.
</process>
