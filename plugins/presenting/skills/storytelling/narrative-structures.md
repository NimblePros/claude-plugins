# Narrative Structures

Seven structures, from the most universal to the most specialized. Pick one before drafting;
mixing two usually produces a story with no clear shape.

---

## 1. Three-Act Structure

The most universal framework for a self-contained narrative.

| Act | Purpose | Typical Length |
|-----|---------|---------------|
| Act I — Setup | Introduce the world, protagonist, and central problem | ~25% |
| Act II — Confrontation | Escalate conflict; protagonist tries and fails; darkest moment | ~50% |
| Act III — Resolution | Climax, breakthrough, new equilibrium | ~25% |

**Use when:** blog posts, case studies, conference talks, product origin stories.

**Example skeleton:**

- *Setup:* "Our payments system processed $50K/day — until one Tuesday it didn't."
- *Confrontation:* Three weeks of debugging: the race condition no one could reproduce, the midnight escalations, the failed hotfix that made it worse.
- *Resolution:* Root cause found, architectural fix shipped, monitoring added — and what the team learned about distributed systems.

---

## 2. The Hero's Journey (Joseph Campbell / Monomyth)

A 12-stage mythic arc. Distilled to its practical core:

1. **Ordinary World** — Establish the protagonist's baseline reality.
2. **Call to Adventure** — Something disrupts the status quo.
3. **Refusal / Reluctance** — Acknowledge why the challenge is hard or scary.
4. **Crossing the Threshold** — The protagonist commits.
5. **Tests and Allies** — Trials, helpers, obstacles.
6. **Ordeal** — The darkest moment; everything is at risk.
7. **Reward** — Victory, insight, transformation.
8. **The Road Back / Return** — Bringing the lesson home.

**Use when:** founder stories, transformational case studies, keynote narratives, long-form essays.

> **Practical tip:** Audiences connect most with step 6 (the ordeal). Spend time here rather than rushing to the reward. The struggle is the story.

**In a technical talk, cast the audience as the hero** and yourself as the guide who has
already made the journey. The talk hands them the map; they take the road back.

---

## 3. The Five-Act Structure (Freytag's Pyramid)

Originally for drama and tragedy; powerful for longer narratives with multiple turning points.

1. **Exposition** — Context and characters
2. **Rising Action** — Complications multiply
3. **Climax** — The decisive moment
4. **Falling Action** — Consequences unfold
5. **Dénouement** — Final resolution and new normal

**Use when:** multi-chapter content, long-form post-mortems, retrospective talks.

---

## 4. The Story Spine (Pixar / improvisational theater)

A fill-in-the-blank scaffold for rapid story construction:

> *Once upon a time…*
> *Every day…*
> *Until one day…*
> *Because of that…*
> *Because of that…* *(repeat for escalation)*
> *Until finally…*
> *And ever since then…*

**Use when:** quickly sketching a story's shape before writing it out, pitches, quick anecdotes.

---

## 5. Problem–Agitate–Solve (PAS)

A persuasion-focused structure common in copywriting and product narratives.

1. **Problem** — Name the pain clearly and specifically.
2. **Agitate** — Deepen the reader's awareness of the problem's cost, consequences, and frustration.
3. **Solve** — Present the resolution, ideally tied to a concrete outcome.

**Use when:** landing pages, sales narratives, product blog posts, demo scripts.

---

## 6. Situation–Complication–Resolution (SCR / McKinsey Pyramid)

A tightly structured analytical narrative for business audiences.

1. **Situation** — Establish what is known and agreed-upon.
2. **Complication** — Introduce what has changed or gone wrong.
3. **Resolution** — Answer: what should be done (or what was done)?

**Use when:** executive briefings, technical reports, architecture decision records, post-mortems.

---

## 7. In Medias Res ("Into the Middle")

Start in the middle of the action — the dramatic moment — then loop back to explain how you got there.

**Example:** Open with the outage in progress, the alarm going off, the on-call engineer staring at a graph. Then rewind: "It started three months earlier when we decided to migrate the database."

**Use when:** presentations, talks, articles where the opening needs immediate energy.

---

## Nesting Structures in a Long Talk

A 45–60 minute talk rarely holds one arc end to end. Use one structure for the whole talk and
smaller ones inside it:

- **Whole talk:** Three-Act or Hero's Journey — the audience's transformation from the problem
  to the new way of working.
- **Each section:** a Story Spine or SCR mini-arc — every section opens with its own small
  problem and closes on a resolution that sets up the next.
- **Anecdotes:** in medias res — drop into the moment, then give the minimum backstory.
