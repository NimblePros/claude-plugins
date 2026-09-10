---
name: live-demos
description: Plan, rehearse, and deliver live coding and product demos in technical talks — demo scripts, checkpoint branches, preparing the machine and IDE for the room, fallback recordings, narrating while typing, and recovering when a demo fails. Use when a talk, workshop, or webinar includes a live demo segment, or when deciding whether a demo should be live or pre-recorded.
user-invocable: false
---

# Live Demos in Technical Talks

## When to Use This Skill

- A talk, workshop, or webinar includes live coding or a live product walkthrough
- Deciding whether a demo should be live, pre-recorded, or replaced with slides
- Writing the demo script, reset procedure, and fallback for each demo segment
- Preparing a laptop, IDE, terminal, and browser for presenting
- Planning what to do when a demo breaks on stage

---

## Live, Recorded, or Slides?

A live demo earns its risk only when *seeing it happen* is the point.

| Situation | Choose |
|-----------|--------|
| Credibility depends on real, unedited behavior (a tool, a workflow, performance) | Live, with a recorded fallback |
| The interesting part takes minutes of waiting (builds, deploys, provisioning) | Recorded and sped up, or live with pre-baked checkpoints |
| Venue network or hardware is unknown | Recorded, or live against local-only services |
| The point is a code *change*, not running software | Slides with step-through code (see the `technical-visuals` skill) |
| Hands-on workshop | Live, at the audience's pace, with every step also written down |

---

## The Demo Script

Write one block per demo segment and keep it in the speaker notes or on a printed card:

```text
DEMO 2 — Add caching                 target 4 min · hard stop 6 min
Goal:      audience sees p95 drop from ~800 ms to under 50 ms
Start:     branch demo/02-start · app running on :5000 · dashboard in browser tab 2
Steps:
  1. Show the slow endpoint in the dashboard     say: "watch the p95 line"
  2. Insert the cache decorator (snippet: cache)  say: why a decorator, not inline
  3. Re-run the load test                         expect: p95 < 50 ms
Fallback:  hidden slide #demo-2-fallback · recording demo-2.mp4
Reset:     git switch -f demo/02-start && docker compose restart api
```

The **goal** line matters most: if the demo fails, you switch to the fallback and still land that goal.

---

## Checkpoints, Not Typing

- **A git branch or tag per step** (`demo/01-start`, `demo/01-done`, `demo/02-start`…). If typing goes wrong, jump to the next checkpoint and keep going.
- **Snippets for anything over ~3 lines** (IDE snippets, live templates). Type only the line that teaches.
- **Pre-warm everything**: restore packages, build once, pull container images, hit each endpoint once so the first request isn't the slow one.
- **A reset script** that takes the demo from any dirty state back to its start. Test it from a *dirty* state, not a clean one.

---

## Preparing the Machine for the Room

| Area | Setting |
|------|---------|
| Resolution | 1920×1080 or lower; mirror the display so you see what they see |
| Editor and terminal fonts | 20pt+ in the editor, same in the terminal; zoom the browser to 150%+ |
| Theme | High contrast; light themes often survive washed-out projectors better than dark ones |
| Distractions | Do Not Disturb / Focus on; chat, email, and notification sources closed; bookmarks bar hidden |
| Privacy | A dedicated browser profile and OS user; secrets in environment variables, never in files you will open on screen |
| Network | Assume no Wi-Fi: local services, cached packages, pulled images; a phone hotspot as backup only |
| Pointing | A screen zoom tool (ZoomIt on Windows, built-in zoom on macOS) or the editor's keystroke-display mode, not waving the mouse |
| Hardware | Charger, display adapters (HDMI and USB-C), and a clicker with fresh batteries |

---

## Narrating While You Demo

- **Tell, show, tell**: say what you are about to do and what to watch for, do it, then name what just happened in one sentence.
- **Never type in silence for more than ~10 seconds.** Talk about *why* while you type the *what*.
- **Read errors aloud** before reacting to them — the audience can't read a stack trace from row 20.
- **Slow down on the payoff.** Pause on the result long enough for the back row to find it.

---

## When It Breaks

1. **One retry, about 30 seconds.** Say what you are checking out loud.
2. **Then switch** to the checkpoint or fallback, and say so plainly: "Let's jump to the finished version." No apology tour.
3. **Land the goal anyway** from the fallback slide or recording.
4. **Don't debug live** for more than a minute unless debugging *is* the talk.

A calm recovery builds more credibility than a flawless demo. Have a recovery line ready (the `comedic-speaking` skill has several).

---

## Rehearsal Checklist

- [ ] Full run-through on the actual laptop, display settings, and (if possible) venue network — or fully offline
- [ ] Reset script tested from a dirty state
- [ ] Each demo timed; anything over its hard stop cut or moved to a recording
- [ ] Fallback screenshots and recordings captured *after* the final rehearsal, so they match what you'll show
- [ ] Demo scripts in the speaker notes, including reset commands
- [ ] Notifications off, secrets off screen, fonts enlarged

## In a reveal.js Deck

Put a **demo cue slide** before each demo (what to watch for, one line), then a hidden fallback slide right after it (`data-visibility="hidden"`, see the `revealjs` skill) with the result screenshot or an embedded recording. Put the demo script and reset command in the cue slide's speaker notes.
