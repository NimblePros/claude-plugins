---
title: "presenting"
description: "Technical presentations end to end: planning, storytelling, humor, code and diagram visuals, live demos, CFP abstracts, and reveal.js slide decks"
summary: "Plan, write, and build technical talks"
date: 2026-09-10
weight: 80
---

Technical presentations end to end — from the CFP abstract to the finished reveal.js deck.
Language-agnostic: the guidance applies to any technical topic and, apart from the `revealjs`
skill, any slide tool.

```bash
claude plugin install presenting@nimblepros
```

This plugin is knowledge plus one agent: seven skills and no slash commands. The skills are
background material Claude loads on demand when you are planning, writing, or building a talk;
you don't invoke them directly.

## Skills

| Skill | One-line summary |
|---|---|
| [multimedia-presentations]({{< ref "multimedia-presentations" >}}) | The entry point: the presentation brief, choosing a format, outlining, rehearsal, and a review checklist |
| [storytelling]({{< ref "storytelling" >}}) | Narrative structures, hooks, tension, pacing, and endings — so a talk reads as a story, not a list |
| [technical-visuals]({{< ref "technical-visuals" >}}) | Code, diagrams, terminal output, and charts that read from the back of the room |
| [live-demos]({{< ref "live-demos" >}}) | Demo scripts, checkpoint branches, machine setup, and recovering when a demo breaks |
| [comedic-speaking]({{< ref "comedic-speaking" >}}) | Humor styles, joke formats, placement, timing, and recovery lines for tech talks |
| [talk-abstracts]({{< ref "talk-abstracts" >}}) | CFP submissions: titles, abstracts, takeaways, bios, and notes for reviewers |
| [revealjs]({{< ref "revealjs" >}}) | Building reveal.js 6 decks: skeleton, themes, Markdown slides, fragments, code, and export |

`multimedia-presentations` is the hub — it links to each of the others for the part of the
talk it covers.

## Agents

**`presentation-deck-creator`** gathers a presentation brief (audience, goal, length, venue,
branding, existing theme) and builds a complete reveal.js deck: a single self-contained HTML
file with speaker notes and timing estimates on every slide, or one that links a project's
shared `theme.css`. It loads `storytelling` for the arc, `technical-visuals` and `live-demos`
for code and demo slides, `comedic-speaking` only when humor is wanted, and `revealjs` for the
implementation — including its rule that any slide with three or more items reveals them one
at a time. It keeps project-scoped memory of brand colors, theme locations, and style
preferences across sessions.

## Requirements

Nothing beyond Claude Code. Decks the agent builds load reveal.js from the jsDelivr CDN, so
presenting them needs a network connection unless you vendor reveal.js locally; decks that load
slides from external Markdown files must be served over HTTP.

## Roadmap

Candidates for this plugin as content is written: `speaker-notes`, `screencast-production`,
a `/presenting:deck-review` command that audits an existing deck against these skills, and a
`presentation-coach` agent for rehearsal feedback.
