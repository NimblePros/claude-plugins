---
title: "commit"
description: "Generate a commit message following project conventions"
summary: "/workflow:commit — conversation-aware Conventional Commits"
date: 2026-08-13
weight: 30
---

> Generate a commit message following project conventions. Use when the user says "commit",
> "commit this", "make a commit", or invokes /commit.

Invoke with `/workflow:commit`.

## What it covers

A three-step commit flow:

1. **Confirm intent** — work item ID (auto-detected from the branch name where possible) and
   commit type (`fix`, `feat`, `refactor`, `chore`, `docs`, `test`, `perf`), asked only when the
   conversation doesn't already make both obvious.
2. **Gather evidence** — `git status`, `git diff -w HEAD`, recent log. Notable opinion: derive
   the message from *conversation context first*, falling back to the diff — the conversation
   knows *why*, the diff only knows *what*.
3. **Confirm and commit** — present the message for approval, then stage and commit. A hard rule:
   never amend a previous commit without explicit approval.

## Ships with

| File | Contents |
|---|---|
| `references/conventions.md` | The full Conventional Commits v1.0.0 reference: type-to-SemVer table, breaking-change syntax, and an anti-patterns list (no file lists, no subject restatement, no "This commit...") |

## Cross-references

`references/conventions.md` is the most complete of the plugin's three Conventional Commits
sources — the [conventional-commit]({{< ref "conventional-commit" >}}) skill covers the same
spec as a prompt template, and the [pr]({{< ref "pr" >}}) skill applies the same title format to
pull requests. Uses `git` only (no `gh`).
