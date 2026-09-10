# presenting

Technical presentations end to end — planning a talk, shaping it into a story, making code and
diagrams readable, running live demos, adding humor, writing the CFP abstract, and building the
deck in reveal.js.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install presenting@nimblepros
```

## Skills

| Skill | What it covers |
|---|---|
| `multimedia-presentations` | The entry point: defining the presentation brief, choosing between slides, live demo, recording, or hybrid, outlining, rehearsal, and a final review checklist. Links to the other skills for depth. |
| `storytelling` | Narrative structures (Three-Act, Hero's Journey, Story Spine, PAS, SCR, in medias res), hooks, tension, pacing, endings, and how to adapt a story for talks, decks, demos, case studies, and post-mortems. |
| `technical-visuals` | Code, architecture diagrams, terminal output, benchmarks, and charts on slides: readability budgets, incremental diagrams, choosing chart types, and accessible color. Tool-agnostic. |
| `live-demos` | Choosing live vs. recorded, demo scripts, checkpoint branches, preparing the machine for the room, narrating while typing, and recovering when a demo breaks. |
| `comedic-speaking` | Humor styles and reusable joke formats that land in tech talks, placement and timing, reading the room, and recovery lines for jokes that don't land. |
| `talk-abstracts` | Conference CFP submissions and session descriptions: titles, abstracts, verb-first takeaways, audience level, speaker bios, and notes for reviewers. |
| `revealjs` | Building reveal.js 6 decks: page skeleton and CDN paths (and 5.x differences), shared CSS-variable themes, Markdown slides, fragments, transitions, auto-animate, code highlighting, speaker notes, and PDF export. |

All skills are background knowledge (`user-invocable: false`): Claude loads them when a task
calls for them rather than exposing them as slash commands.

## Agents

| Agent | Purpose |
|---|---|
| `presentation-deck-creator` | Gathers a presentation brief and builds a complete, self-contained reveal.js deck with speaker notes on every slide, applying the skills above. Keeps project-scoped memory of brand, theme, and style preferences. |

## Roadmap

Candidates for this plugin as content is written: `speaker-notes`, `screencast-production`,
a `/presenting:deck-review` command that audits an existing deck against these skills, and a
`presentation-coach` agent for rehearsal feedback.
