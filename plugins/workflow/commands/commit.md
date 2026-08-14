---
description: Stage and commit changes following the project's conventional commit format.
argument-hint: [type(scope): message] [--work-item <id>]
allowed-tools: Bash(git *)
---

<objective>
Execute the `commit` skill: read the current diff, confirm the commit type and work item ID, present the message for approval, then stage and commit.

`$ARGUMENTS` may contain an explicit commit type, scope, description, or work item ID — prefer supplied values over auto-detection. If omitted, infer everything from the branch name and diff.
</objective>

<context>
Current branch: !`git branch --show-current`
Staged: !`git diff --cached --stat`
Unstaged: !`git diff --stat`
Recent commits: !`git log --oneline -5`
</context>

<process>
Follow the `commit` skill.
</process>
