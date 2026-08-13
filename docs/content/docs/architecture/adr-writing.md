---
title: "adr-writing"
description: "Write Architecture Decision Records: naming, numbering, lifecycle, and duplicate resolution"
summary: "The full ADR authoring workflow"
date: 2026-08-13
weight: 10
---

> Write Architecture Decision Records (ADRs) for a project's documentation. Covers file naming,
> frontmatter, required sections, status lifecycle, index maintenance, and detecting/fixing
> duplicate ADR numbers from parallel branches. Load before creating, revising, or superseding
> any ADR, or when asked to check for duplicate ADR numbers or fix the ADR list.

## What it covers

The full authoring workflow, with one meta-rule above all: **existing repository conventions win
over this skill** — read a few recent ADRs first and match their style.

- **Locating and numbering** — find the repo's ADR directory, then derive the next number from
  index titles and `title:` frontmatter rather than filenames (filenames drift; renaming changes
  published URLs). Files are `adr-NNN-short-slug.md`.
- **Required sections** — Context, Problem Statement, Decision, Options Considered (each option
  either `(chosen)` or with a codebase-verified **Rejected because:**), Rationale, Consequences
  split Positive/Negative, Open Issues, Future Considerations, Related Links. The skill insists
  on honest negatives: "an ADR with no negatives reads as unexamined."
- **Status lifecycle** — `Proposed` → `Active` → `Superseded`, handled append-only: never delete
  or renumber old ADRs; link the old status line to the successor.
- **Duplicate numbers from parallel branches** — a detection-and-resolution procedure: the
  first-merged ADR keeps the number, the later one is renumbered to highest+1 (never filling
  gaps) in a single atomic commit that updates the file, the index, and every repo-wide
  reference.

## Ships with

| File | Contents |
|---|---|
| `template.md` | A blank ADR skeleton with prompts for every section |
| `examples/full-example-adr.md` | A complete worked ADR — the reference for tone and rigor |
| `examples/superseded-example-adr.md` | Demonstrates the superseded status line |
| `examples/example-index.md` | A sample ADR index page |

## Cross-references

Standalone — references no other skills or agents, though the `technical-writer` agent in this
plugin covers ADRs among the document types it produces. Uses `gh pr list` and `git log` when
resolving duplicate numbers across branches. This repository's own
[devdocs/ADRs](https://github.com/NimblePros/claude-plugins/tree/main/devdocs/ADRs) follow this
skill's format.
