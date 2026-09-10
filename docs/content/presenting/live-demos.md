---
title: "live-demos"
description: "Plan, rehearse, and deliver live coding and product demos in technical talks"
summary: "Demos that survive the stage"
date: 2026-09-10
weight: 40
---

> Plan, rehearse, and deliver live coding and product demos in technical talks — demo scripts,
> checkpoint branches, preparing the machine and IDE for the room, fallback recordings,
> narrating while typing, and recovering when a demo fails. Use when a talk, workshop, or
> webinar includes a live demo segment, or when deciding whether a demo should be live or
> pre-recorded.

## What it covers

- **Live, recorded, or slides?** — a decision table for when a live demo earns its risk.
- **The demo script** — a per-segment template with a goal, start state, steps, fallback, and
  reset command.
- **Checkpoints, not typing** — a git branch per step, snippets, pre-warming, and a tested reset
  script.
- **Preparing the machine** — resolution, fonts, theme, notifications, privacy, offline
  operation, pointing tools, and hardware.
- **Narrating and recovering** — tell-show-tell, reading errors aloud, and the one-retry rule
  before switching to the fallback.
- **A rehearsal checklist** and how to wire demo cue and hidden fallback slides into a reveal.js
  deck.

## Ships with

The `SKILL.md` only.

## Cross-references

Loaded by the `presentation-deck-creator` agent when a talk includes a demo. Points to
`technical-visuals` for code shown on slides, `comedic-speaking` for recovery lines, and
`revealjs` for hidden fallback slides.
