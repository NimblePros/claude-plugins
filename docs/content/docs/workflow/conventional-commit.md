---
title: "conventional-commit"
description: "The Conventional Commits reference for commit messages and PR titles"
summary: "The structured format behind commits and PR titles"
date: 2026-08-13
weight: 40
---

> Reference material for the Conventional Commits format used across the workflow plugin.
> Not an installable skill — loaded by the `commit` skill and cited by `pr-agent` and the `pr`
> skill for PR title format requirements.

## What it covers

A reference page (not an installable skill): a structured scaffold
for building a commit message — type, optional scope, imperative-mood description, body, footer
— plus examples and validation rules (type must come from the allowed set; description is
required). It explicitly notes that the same `<type>: <description>` structure doubles as the
**PR title** format, which is why the PR-facing pieces of this plugin point back here.

## Cross-references

The `pr-agent` cites this skill by name for its draft-PR title format, and the
[pr]({{< ref "pr" >}}) skill enforces the same conventional title (with a 65-character cap). It
overlaps heavily with the fuller reference in
[commit]({{< ref "commit" >}})'s `references/conventions.md` — when in doubt, treat that file as
canonical for message *content* and this skill as the quick template. Not slash-invocable; no
supporting files; uses plain `git`.
