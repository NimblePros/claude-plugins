---
title: "technical-visuals"
description: "Code, architecture diagrams, terminal output, benchmarks, and charts that read from the back of the room"
summary: "Readable technical content on slides"
date: 2026-09-10
weight: 30
---

> Present code, architecture diagrams, terminal output, benchmarks, and charts on slides so they
> read from the back of the room — how much code fits, font sizes, highlighting, building
> diagrams incrementally, choosing chart types, and accessible color. Use when a technical talk
> or slide deck includes code samples, system diagrams, CLI output, performance numbers, or data
> visualizations, in any slide tool.

## What it covers

- **A readability budget** — lines of code, font size, line length, diagram boxes, and chart
  series per slide, plus the "back-row test".
- **Code on slides** — show the minimum, use real compiling code, highlight instead of
  shrinking, and never screenshot code.
- **Architecture diagrams** — one abstraction level per diagram, incremental builds, verb-labeled
  arrows, SVG export, and pre-rendered text diagrams.
- **Terminal output, data, and benchmarks** — trimming output, conclusion-first chart titles,
  picking the chart type, and stating benchmark conditions.
- **Accessible color** and a review checklist.

## Ships with

The `SKILL.md` only. It is tool-agnostic; the reveal.js mechanics for code live in the
`revealjs` skill's `code-slides.md`.

## Cross-references

Loaded by the `presentation-deck-creator` agent for code, diagram, and data slides; `live-demos`
points here when a code change is better shown on slides than live.
