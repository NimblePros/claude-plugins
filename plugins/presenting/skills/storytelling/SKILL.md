---
name: storytelling
description: Craft engaging narratives using classic and modern storytelling techniques — Three-Act, Hero's Journey, Story Spine, Problem–Agitate–Solve, Situation–Complication–Resolution, in medias res — plus hooks, tension, character, pacing, and endings. Use when structuring a technical talk, slide deck, blog post, case study, or post-mortem so it reads as a story rather than a list of facts.
user-invocable: false
---

# Storytelling Skill

## When to Use This Skill

Use this skill when:

- Writing a blog post, talk, or article that needs to engage and retain an audience
- Building a presentation that should feel like a narrative, not a bullet-point dump
- Crafting a case study, user story, or technical post-mortem
- Writing an origin story, company narrative, or founder's journey
- Turning dry technical or instructional content into something people actually read
- Creating product demos, sales stories, or conference keynote narratives

## Reference Files

- [narrative-structures.md](narrative-structures.md): The seven structures in detail, with skeletons and examples, plus how to nest them in a long talk
- [context-guidance.md](context-guidance.md): How to adapt a story for talks, blog posts, case studies, slide decks, live demos, and post-mortems

---

## Core Principles of Engaging Storytelling

1. **Conflict is the engine** — No tension, no story. Every compelling narrative has something at stake.
2. **Character before concept** — Root abstract ideas in a person, team, or organization the audience can follow.
3. **Show, don't tell** — Replace assertions ("it was hard") with scenes and specific detail ("the deploy failed at 2 a.m., Friday before a holiday").
4. **Earn the resolution** — The payoff must feel proportional to the struggle. Don't shortcut the hard part.
5. **One clear throughline** — Every scene, fact, and detail should serve the central question the story is answering.
6. **Specificity beats generality** — "A real estate startup in Austin" is more vivid than "a company." Names, numbers, and sensory detail create believability.

---

## Choosing a Structure

| Structure | Shape | Best for |
|-----------|-------|----------|
| Three-Act | Setup → confrontation → resolution | Blog posts, case studies, conference talks |
| Hero's Journey | Ordinary world → ordeal → return with the lesson | Keynotes, founder stories, transformation case studies |
| Five-Act (Freytag) | Exposition → rising action → climax → falling action → dénouement | Long-form post-mortems, retrospectives |
| Story Spine | "Once upon a time… until one day… because of that…" | Sketching a story fast, pitches, anecdotes |
| Problem–Agitate–Solve | Name the pain → deepen it → resolve it | Product posts, demo scripts, sales narratives |
| Situation–Complication–Resolution | Known → what changed → what to do | Executive briefings, reports, ADRs |
| In medias res | Open on the dramatic moment, then rewind | Talks and articles that need an energetic opening |

See [narrative-structures.md](narrative-structures.md) for each structure's steps and a worked skeleton.

---

## Techniques for Engaging Stories

### Opening Hooks

The first 30 seconds (or two sentences) determine whether the audience stays. Use one of:

- **Provocative question:** "What would you do if your largest customer's data disappeared?"
- **Surprising statistic:** "73% of microservice migrations end up as distributed monoliths." *(Only use a statistic you can cite — an invented number destroys credibility the moment someone checks it.)*
- **Vivid scene:** "At 3 a.m., the Slack message arrived: 'Production is down.'"
- **Bold claim:** "Everything you've been taught about REST API design is backward."
- **Anecdote drop:** Jump straight into a moment, no preamble.

Avoid: "Today I'm going to talk about X." It signals a lecture, not a story.

### Building and Sustaining Tension

Tension is the gap between what the protagonist wants and what stands in the way.

- **Raise the stakes explicitly** — Tell the audience what failure would cost.
- **False hope / setback cycles** — Let the protagonist almost succeed before failing again (Act II engine).
- **Clock / deadline** — Add a time constraint. "They had 48 hours before the contract would expire."
- **Dramatic irony** — Let the audience know something the protagonist doesn't.
- **Withhold the answer** — Ask the central question early; delay the answer.

### Character and Relatability

Even non-fiction storytelling needs a protagonist the audience can root for.

- **Give them a clear want** — What does the character want most in this story?
- **Give them a flaw relevant to the conflict** — Overconfidence, inexperience, etc.
- **Show inner life** — Brief glimpses of doubt, frustration, or hope create empathy.
- **Use real names and details** — Anonymized composites are fine, but vague archetypes are not.

### Pacing

- **Short sentences accelerate.** Long, complex sentences, with multiple subclauses, slow the reader down and signal reflection or exposition.
- **White space is pacing.** In presentations: silence. In text: paragraph breaks.
- **Scene vs. summary:** Use scenes (moment-by-moment detail) for high-stakes moments. Use summary for transitions and low-stakes material.
- **End scenes on movement** — Avoid ending a section on a static explanation. End it at the moment something changes.

### The Moment of Insight (The "So What")

Every story needs a moment where the audience understands why they should care about what they just heard. Structure it explicitly:

1. **What happened** — The factual summary.
2. **What it means** — The interpretation or lesson.
3. **So what / now what** — What should the audience do or think differently?

Don't assume the audience will extract the lesson themselves. State it.

### Endings That Land

Weak endings summarize. Strong endings reframe.

- **Full circle** — Return to the opening image or question, now transformed by what the audience knows.
- **Call to action** — An invitation that flows naturally from the story's lesson.
- **Earned emotional beat** — A quiet, specific moment that lands the weight of the journey without overselling it.
- **Reframe** — Reveal that the story was "about" something deeper than the surface narrative.

Avoid: trailing off, over-explaining the moral, or apologizing for the ending.

---

## Common Storytelling Mistakes to Avoid

| Mistake | Fix |
|---------|-----|
| Starting with context before the hook | Start with the dramatic moment; earn the backstory |
| Skipping the struggle (journey to Act III too fast) | Dwell in complication; the ordeal is where empathy is built |
| Ending with a summary instead of a punch | End on the transformed state or a call to action |
| Too many protagonists or plot threads | Commit to one throughline; cut subplots that don't serve it |
| Passive voice and abstract language | Name the actor; use concrete verbs |
| Telling emotions rather than showing them | Replace "they were frustrated" with the action that reveals the emotion |
| Moralizing | Let the story carry the lesson; state it once, cleanly, and stop |

---

## Quick-Start Checklist

Before writing, answer:

- [ ] Who is the protagonist, and what do they want?
- [ ] What is the central conflict or obstacle?
- [ ] What structure best fits this content? (Three-Act, Hero's Journey, PAS, SCR, etc.)
- [ ] What is the hook (first sentence/slide/moment)?
- [ ] What is the darkest moment or biggest setback?
- [ ] What changes by the end — for the protagonist, and for the audience's understanding?
- [ ] What is the single sentence the audience should walk away remembering?
