---
title: "architecture"
description: "Architecture decisions and technical documentation: writing ADRs and producing clear developer-facing docs"
summary: "ADR writing and the documentation that records decisions"
date: 2026-08-13
weight: 60
---

Architecture decisions and the documentation that records them. Language-agnostic by design —
architecture philosophy does not live inside a language plugin.

```bash
claude plugin install architecture@nimblepros
```

## Skills

| Skill | One-line summary |
|---|---|
| [adr-writing]({{< ref "adr-writing" >}}) | The full ADR authoring workflow: naming, numbering, required sections, status lifecycle, and resolving duplicate numbers from parallel branches |

## Agents

**`technical-writer`** creates and reviews technical documentation — API docs, READMEs, guides,
tutorials, ADRs, changelogs — for clarity, accuracy, and consistency. It follows a
review-code-change → update-docs → lint → verify workflow with an explicit quality checklist,
and keeps project-scoped memory of a project's documentation conventions and terminology across
sessions. Its worked examples assume a Hugo documentation site (Geekdoc theme) and a CLI
project; adapt the paths and commands to your own documentation tooling.

The agent and skill are independent — `technical-writer` does not load `adr-writing` — but they
cover the same territory from two directions: the skill defines *what a good ADR is*, and the
agent is a general documentation workhorse that can produce one.

## Roadmap

Candidates for this plugin as content is written: `modular-monolith`, `vertical-slice`,
`clean-architecture`, `architecture-review`, and a `dependency-analyzer` agent.

## Seen in the wild

This repository practices what the plugin preaches: the marketplace restructure is recorded in
[ADR 0001](https://github.com/NimblePros/claude-plugins/blob/main/devdocs/ADRs/0001-2026-08-13-multi-plugin-marketplace-structure.md),
written in the format this skill defines.
