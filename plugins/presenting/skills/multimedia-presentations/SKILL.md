---
name: multimedia-presentations
description: Plan and refine multimedia presentations such as slide decks, demos, recorded videos, and conference talks, with practical guidance on structure, visuals, rehearsal, and delivery. Use when starting, restructuring, or reviewing a talk, webinar, demo, or screencast.
user-invocable: false
---

# Multimedia presentations

## When to Use This Skill

Use this skill when:

- Creating a new slide deck, demo, webinar, or screencast
- Turning a technical topic into a presentation with a clear audience takeaway
- Deciding whether content works better as slides, a live demo, a recording, or a hybrid format
- Tightening an existing presentation that feels too dense, too long, or too abstract
- Preparing speaker notes, rehearsal checklists, or delivery guidance

## Reference Files

- [presentation-patterns.md](presentation-patterns.md): Planning frameworks, pacing guidance, and structure patterns for talks, demos, and videos

## Related Skills

This skill is the entry point for planning. The other skills in this plugin go deeper on one part each:

| Need | Skill |
|------|-------|
| A narrative arc and an opening hook | `storytelling` |
| Code, diagrams, terminal output, or charts on slides | `technical-visuals` |
| A live coding or product demo | `live-demos` |
| Humor and lighter moments | `comedic-speaking` |
| Building the deck in reveal.js | `revealjs` |
| A CFP submission or session description | `talk-abstracts` |

To have a complete reveal.js deck built from a brief, use the `presentation-deck-creator` agent.

## Core Principles

1. **Design for the audience** - Start with who the presentation is for, what they already know, and what they should do differently afterward.
2. **One takeaway per segment** - Each section should teach, persuade, or demonstrate a single clear idea.
3. **Show, don't just tell** - Prefer examples, screenshots, diagrams, and demos over dense text.
4. **Make the narrative obvious** - Open with the problem, move through the approach, and close with the outcome or next action.
5. **Rehearse the risky parts** - Practice transitions, timing, demos, and any moments that depend on tooling or networking.
6. **Optimize for the medium** - Slides, live demos, and recorded videos each need different pacing, fidelity, and redundancy.

---

## Workflow

### 1. Define the presentation brief

Capture these before drafting content:

- Audience: role, technical depth, and likely context
- Goal: inform, persuade, teach, align, or sell
- Format: slide deck, live demo, recorded walkthrough, or mixed session
- Duration: target time plus how much Q&A or buffer exists
- Outcome: the specific conclusion, action, or decision you want from the audience

If the brief is vague, pause and clarify it before writing detailed content.

### 2. Choose the right delivery shape

- **Slide deck** - Best when the talk needs structure, visuals, comparison, or room-level pacing.
- **Live demo** - Best when credibility depends on showing real behavior or workflow.
- **Recorded video** - Best for repeatable asynchronous training or product walkthroughs.
- **Hybrid** - Best when the audience needs both framing and proof, such as slides for context and a short demo for evidence.

### 3. Build the outline first

Use a simple story arc:

1. Hook - Why this matters now
2. Problem - What is hard, broken, or expensive
3. Approach - What you changed, built, or learned
4. Evidence - Demo, data, before/after, or case study
5. Takeaway - What the audience should remember or do next

### 4. Prepare supporting assets

- Keep slides visually sparse and speaker-friendly
- Pre-stage demo data, accounts, feature flags, and fallback screenshots
- For video, script intros/outros and remove avoidable dead time
- Create speaker notes only for transitions, numbers, and phrasing you must land precisely

### 5. Rehearse for delivery

- Time the full run, not just the main content
- Practice demos on the same setup you will actually use
- Test screen sharing, audio, resolution, and browser zoom
- Trim content before the talk instead of rushing during it

---

## Review Checklist

Before a presentation is done, verify:

- The audience and desired outcome are explicit
- Every section supports the core takeaway
- Slides are readable from a distance and avoid paragraph walls
- Demos have a fallback plan if the environment fails
- The presentation fits comfortably within the available time
- The ending clearly summarizes the point and next action
