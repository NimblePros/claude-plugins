---
name: technical-visuals
description: Present code, architecture diagrams, terminal output, benchmarks, and charts on slides so they read from the back of the room — how much code fits, font sizes, highlighting, building diagrams incrementally, choosing chart types, and accessible color. Use when a technical talk or slide deck includes code samples, system diagrams, CLI output, performance numbers, or data visualizations, in any slide tool.
user-invocable: false
---

# Technical Visuals for Slides

Tool-agnostic guidance for the visuals technical talks depend on. For reveal.js mechanics
(highlight plugin, line-highlight steps, auto-animate), see the `revealjs` skill's
`code-slides.md`.

## Readability Budget

| Element | Limit |
|---------|-------|
| Code lines visible at once | ~10–15; beyond that, split the slide or step through it |
| Code font size | ≥ ~20px on a 1280×720 canvas (≈ 24pt in PowerPoint/Keynote at 16:9) |
| Code line length | ≤ ~60 characters; shorten names or wrap deliberately |
| Words on a slide (excluding code) | ~20 or fewer, with a claim as the title |
| Diagram boxes visible at once | ~7 or fewer; build up the rest |
| Chart series | 4–5 at most; emphasize the one that matters |

**The back-row test:** view the slide at 25% zoom on a laptop. If you can't read it, neither can the back row.

---

## Code on Slides

- **Show the minimum that proves the point.** Delete imports, `using` directives, logging, and error handling unless they *are* the point. Mark removals with `// …`.
- **Use real code that compiles and was run.** Developers spot bugs on slides instantly, and it derails the talk. Link the repository on the final slide.
- **Rename for the slide.** Short, meaningful names beat production names that need explaining.
- **Highlight, don't shrink.** Show the snippet, then emphasize the 1–3 lines under discussion and dim the rest; step through larger snippets one region per click.
- **Before/after:** side by side only when both fit in ~12 lines; otherwise use two slides or animate one into the other.
- **Never use screenshots of code.** They blur when scaled, can't be copied from shared slides, and ignore your theme. Use highlighted text.
- **Pick a monospace font** that distinguishes `0`/`O` and `1`/`l`/`I`. Consider disabling ligatures — `=>` and `!=` rendered as single glyphs confuse people who haven't seen them.

---

## Architecture Diagrams

- **One level of abstraction per diagram.** Don't mix system context, containers, and classes on one slide (the [C4 model](https://c4model.com/) levels are a good checklist).
- **Build incrementally.** Start with one or two boxes and add components per click, in the order you talk about them.
- **Label arrows with verbs** ("publishes `OrderPlaced`", "reads from"), and keep flow direction consistent — e.g. requests always left-to-right.
- **Dim what you're not discussing** rather than showing a new diagram; the audience keeps their bearings.
- **Use color for grouping, never as the only signal** — pair it with shape, position, or labels.
- **Export SVG**, not PNG, so diagrams stay crisp at any projector resolution. Keep the diagram source (Mermaid, PlantUML, D2, Excalidraw, draw.io) next to the deck so it can be edited later.
- **Pre-render text-based diagrams** (e.g. `mmdc -i flow.mmd -o flow.svg` with mermaid-cli) instead of rendering them in the browser at show time — one less thing that can fail on stage.

---

## Terminal and Tool Output

- Trim to the lines that matter; replace the rest with a single `…` line.
- Use a clean prompt (`$ `) — no usernames, hostnames, or long paths.
- Emphasize the line that carries the message (the pass count, the error, the timing).
- Prefer a highlighted text block over a screenshot, for the same reasons as code.

---

## Data, Benchmarks, and Charts

**Put the conclusion in the title:** "Caching cut p95 latency 20×", not "Latency results".

| Message | Chart |
|---------|-------|
| Compare values across categories | Bar (horizontal when labels are long) |
| Change over time | Line |
| Distribution or variance | Histogram or box plot |
| Part of a whole | Stacked bar; a pie only with 3 or fewer slices |
| Correlation | Scatter |

- Start bar-chart axes at zero; label units on every axis.
- Label the data directly instead of using a legend the audience has to decode.
- Show absolute numbers alongside percentages ("40% faster: 500 ms → 300 ms").
- **Benchmarks:** state the hardware, runtime and library versions, and iteration count; show variance (error bars or ranges); link the raw results. Never compare numbers measured on different machines as if they were comparable.

---

## Accessible Color

- Text contrast of at least 4.5:1 (WCAG AA); aim higher, because projectors wash colors out.
- Never rely on red vs. green alone — add icons, labels, or position.
- Use a colorblind-safe palette (for example, Okabe–Ito) for charts and diagram groupings.
- Check the deck in grayscale: if meaning survives, color is doing its job as reinforcement.

---

## Review Checklist

- [ ] Every code slide passes the back-row test and stays within the line budget
- [ ] Code samples compile, were run, and the repository is linked
- [ ] Each diagram stays at one level of abstraction and builds up incrementally
- [ ] Every chart's title states its conclusion; axes are labeled and honest
- [ ] Benchmark conditions are stated
- [ ] No meaning depends on color alone
