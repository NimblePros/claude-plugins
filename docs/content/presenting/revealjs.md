---
title: "revealjs"
description: "Build and edit reveal.js 6 HTML slide decks: skeleton, themes, Markdown slides, fragments, code, and export"
summary: "reveal.js decks that work first time"
date: 2026-09-10
weight: 70
---

> Build and edit reveal.js HTML slide decks — the reveal.js 6 page skeleton and CDN paths,
> differences from 5.x decks, shared CSS-variable themes across decks, Markdown-authored slides,
> fragments, transitions, auto-animate, code highlighting, speaker notes, and PDF export. Use
> when creating, editing, theming, or debugging a reveal.js presentation.

## What it covers

- **Version and CDN paths** — reveal.js 6 moved every distributable file (`dist/reveal.mjs`,
  `dist/plugin/<name>.mjs`). A table maps each 6.x path to its 5.x equivalent, and existing decks
  keep the version they already use.
- **A quick-start skeleton** that works when opened straight from disk.
- **The default fragment rule** — any slide with three or more discrete items reveals them one
  per click; hero and divider slides are exempt.
- **Speaker notes, recommended configuration, and common patterns** — section dividers, hero
  images, vertical slides, and hidden backup slides for demo fallbacks.
- **Common gotchas** — wrong-version paths, external Markdown over `file://`, shadowing `Math`,
  and unreadable code.

## Ships with

The `SKILL.md` is a hub; detail lives in four supporting files:

| File | Contents |
|---|---|
| `theming.md` | Built-in themes and their `--r-*` variables, a shared `theme.css` token system across decks, per-deck overrides, and layout helpers (`r-stack`, `r-fit-text`, `r-stretch`) |
| `markdown-slides.md` | Inline and external Markdown slides, separators, notes, slide/element attributes, and serving requirements |
| `code-slides.md` | Highlight plugin setup, escaping, step-through line highlights, morphing code with auto-animate, and code sizing |
| `animations-reference.md` | Transitions, every fragment style, staggering, auto-animate, CSS animations, scroll view, PDF export, keyboard shortcuts, and plugin imports |

## Cross-references

The implementation layer for the `presentation-deck-creator` agent. For *what* to put on code
slides, see [technical-visuals]({{< ref "technical-visuals" >}}).
