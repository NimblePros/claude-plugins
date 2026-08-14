---
description: Run a parallel multi-agent code review on the current diff or a specific pull request.
argument-hint: [pr-number]
allowed-tools: Bash(git *), Bash(gh *)
---

<objective>
Execute the `review` skill: dispatch parallel agents (bug scan, git history, shared-code impact, conventions) against the specified PR or the current local diff, then synthesise and present findings.

`$ARGUMENTS` is an optional PR number. If omitted, review the local diff against `main`.
</objective>

<context>
Current branch: !`git branch --show-current`
</context>

<process>
Follow the `review` skill.
</process>
