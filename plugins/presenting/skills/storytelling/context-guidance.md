# Context-Specific Storytelling Guidance

How the core principles adapt to each kind of content.

---

## Technical Talks and Conference Presentations

- Lead with the problem, not the solution. Audience buy-in requires feeling the pain before the fix.
- Use one protagonist (a team, a company, a system) rather than jumping between examples.
- Every code snippet, diagram, and demo should be a plot point — it should advance understanding, not just show off capability.
- Reserve the last 10% for "what I'd do differently" — it's the most memorable and honest part of any technical story.
- Signpost the structure. Unlike readers, listeners can't skim ahead; a short "three parts: why, how, what broke" at the start and a callback at each transition keeps them oriented.

## Blog Posts and Articles

- The headline is the hook; the first paragraph is the proof the hook was worth following.
- Use subheadings as a secondary narrative — a reader skimming subheadings should understand the story's shape.
- Specific numbers and dates signal credibility: "after 14 failed deploys" beats "after many attempts."
- End with an open question or invitation when you want comments and engagement.

## Case Studies

- Structure: *Before state → Trigger/decision → Journey/challenges → After state → Lessons.*
- Quantify the before and after state whenever possible.
- Include a complication — a pure success story reads as marketing. A real story includes what was harder than expected.
- Let the subject's voice appear (quotes, their framing of the problem) rather than narrating everything in third person.

## Presentations and Slide Decks

- Each slide is a beat, not a document. One idea per slide.
- The slide title should advance a *claim* ("Latency tripled after migration"), not a *topic* ("Latency Results").
- Use images and diagrams to show; use words to tell the audience what they just saw.
- Build tension visually: before/after comparisons, rising graphs, cascading failures.
- Read only the slide titles in order. If they don't tell the story on their own, the deck's structure is wrong — fix the titles before polishing the visuals.

## Live Demos

- A demo is Act II made visible: state what the audience is about to see and why it matters *before* you start typing.
- Stage one planned obstacle — a failing test, a slow query — and resolve it on stage. A demo where everything works first time has no tension.
- End the demo by naming what just happened in one sentence; don't assume the audience followed every keystroke.

## Post-Mortems and Incident Reports

- Avoid blame; focus on the system, not the individual.
- Follow the timeline like a thriller: what was known at each moment, what was tried, why it failed.
- The "lesson learned" section should be specific and actionable, not platitudes.
- Acknowledge the human experience: the stress, the uncertainty, the coordination failures. It makes the technical lessons more memorable.
