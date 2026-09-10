---
name: revealjs
description: Build and edit reveal.js HTML slide decks — the reveal.js 6 page skeleton and CDN paths, differences from 5.x decks, shared CSS-variable themes across decks, Markdown-authored slides, fragments, transitions, auto-animate, code highlighting, speaker notes, and PDF export. Use when creating, editing, theming, or debugging a reveal.js presentation.
user-invocable: false
---

# reveal.js Presentations

## Reference Files

- [theming.md](theming.md): Shared `theme.css` design tokens across decks, per-deck overrides, built-in themes and their `--r-*` variables, and layout helpers (`r-stack`, `r-fit-text`, `r-stretch`)
- [markdown-slides.md](markdown-slides.md): Authoring slides in inline or external Markdown — separators, notes, slide and element attributes, and serving requirements
- [code-slides.md](code-slides.md): Syntax highlighting, step-through line highlights, escaping, and morphing code with auto-animate
- [animations-reference.md](animations-reference.md): Transitions, fragment styles, auto-animate, custom CSS animations, scroll view, PDF export, keyboard shortcuts, and plugin imports

---

## Version and CDN Paths

New decks target **reveal.js 6.x** (6.0.2 at the time of writing; pin an exact version in every URL). reveal.js 6 moved its distributable files, so paths copied from older decks or tutorials fail — a 404 on the core module leaves a blank page with no visible error.

| File | 6.x path | 5.x path |
|------|----------|----------|
| Core ES module | `dist/reveal.mjs` | `dist/reveal.esm.js` |
| Core script (global `Reveal`) | `dist/reveal.js` | `dist/reveal.js` |
| Plugin ES module | `dist/plugin/markdown.mjs` | `plugin/markdown/markdown.esm.js` |
| Plugin script (global `RevealMarkdown`) | `dist/plugin/markdown.js` | `plugin/markdown/markdown.js` |
| Highlight theme CSS | `dist/plugin/highlight/monokai.css` | `plugin/highlight/monokai.css` |
| Core CSS and themes | `dist/reset.css`, `dist/reveal.css`, `dist/theme/black.css` | unchanged |

**When editing an existing deck, keep the version it already uses** and match its path style. Never mix 5.x and 6.x paths in one deck. Upgrade only when asked, following <https://revealjs.com/upgrading/>.

---

## Quick Start Structure

```html
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Talk Title</title>
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/reset.css">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/reveal.css">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/theme/black.css">
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/highlight/monokai.css">
  <link rel="stylesheet" href="theme.css">  <!-- optional shared theme; see theming.md -->
</head>
<body>
  <div class="reveal">
    <div class="slides">
      <section>
        <h1>Talk Title</h1>
        <aside class="notes">Opening hook. ~1 min.</aside>
      </section>
    </div>
  </div>
  <script type="module">
    import Reveal from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/reveal.mjs';
    import RevealHighlight from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/highlight.mjs';
    import RevealNotes from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/notes.mjs';

    Reveal.initialize({
      hash: true,            // slide number in the URL, so refresh keeps your place
      slideNumber: 'c/t',    // current/total
      width: 1280,           // fixed 16:9 canvas; reveal.js scales it to the screen
      height: 720,
      transition: 'slide',
      plugins: [RevealHighlight, RevealNotes],
    });
  </script>
</body>
</html>
```

This works when opened straight from disk (`file://`). Loading slides from external Markdown files does not — see Common Gotchas.

---

## Default Fragment Behavior (Rule)

**Any slide with 3 or more discrete items must use fragments so items appear on click, not all at once.** This applies to:

- Bullet/list items (`<li>`)
- Info cards, warning cards, or any repeated card element
- Pros/cons or observation rows
- Grid cells containing distinct points

Apply `class="fragment"` directly to each item element. Do not wrap items in an extra container just for fragments.

```html
<!-- List items -->
<ul>
  <li class="fragment">First point</li>
  <li class="fragment">Second point</li>
  <li class="fragment">Third point</li>
</ul>

<!-- Cards in a grid -->
<div style="display: grid; grid-template-columns: repeat(2, 1fr); gap: 16px;">
  <div class="info-card fragment">...</div>
  <div class="info-card fragment">...</div>
  <div class="info-card fragment">...</div>
  <div class="info-card fragment">...</div>
</div>

<!-- Observation rows -->
<div class="obs-pro fragment">...</div>
<div class="obs-con fragment">...</div>
```

Slides with 1–2 items, or slides that are purely decorative/hero/divider slides, should **not** use fragments.

For fragment animation styles (`fade-up`, `highlight-red`, `strike`, …) and ordering with `data-fragment-index`, see [animations-reference.md](animations-reference.md).

---

## Speaker Notes

```html
<section>
  <h2>Slide Title</h2>
  <aside class="notes">
    Key point to land. Transition line into the next slide. ~2 min.
  </aside>
</section>
```

Press `S` to open the speaker view (requires the Notes plugin). Write notes as cues, not a script: the point to land, the transition sentence, any number you must say exactly, and an estimated duration. In Markdown slides, notes follow a `Notes:` line — see [markdown-slides.md](markdown-slides.md).

---

## Recommended Configuration

```js
Reveal.initialize({
  hash: true,               // update URL on slide change
  slideNumber: 'c/t',       // current/total
  width: 1280,
  height: 720,
  margin: 0.04,
  minScale: 0.2,
  maxScale: 2.0,

  // Navigation
  controls: true,           // 'speaker' shows arrows only in the speaker view (6.x)
  progress: true,
  keyboard: true,
  overview: true,

  // Export
  showNotes: false,         // true or 'separate-page' to include notes in a PDF export
  pdfSeparateFragments: false,

  plugins: [RevealMarkdown, RevealHighlight, RevealNotes],
});
```

---

## Common Patterns

**Section divider slide** (full-bleed dark):

```html
<section class="section-divider" data-background-color="#0A1520">
  <h1>Part Two</h1>
</section>
```

**Hero slide with background image:**

```html
<section data-background-image="photo.jpg"
         data-background-opacity="0.2"
         data-background-size="cover">
  <h1>Title Over Image</h1>
</section>
```

**Nested vertical slides** (optional detail the presenter can skip with `→`):

```html
<section>
  <section>Horizontal parent</section>
  <section>Vertical child 1</section>
  <section>Vertical child 2</section>
</section>
```

**Hidden backup slide** (reachable by link or from the overview, skipped in normal navigation) — useful for demo fallbacks:

```html
<section id="demo-fallback" data-visibility="hidden">
  <img src="demo-result.png" alt="Expected output of the deploy demo">
</section>
```

---

## Common Gotchas

| Symptom | Cause | Fix |
|---------|-------|-----|
| Blank page, no slides | Module path from the wrong version (e.g. `reveal.esm.js` on 6.x) | Use the path table above; check the browser console for a 404 |
| External Markdown slides don't load | Opened from `file://`; browsers block `fetch` of local files | Serve the folder: `npx serve` or `python -m http.server`, then open `http://localhost:…` |
| `Math.floor is not a function` in the init script | Imported the math plugin as `Math`, shadowing the global | Import it as `RevealMath` and use `RevealMath.KaTeX` |
| Code is unreadable at the back of the room | Too many lines shrunk to fit | Split the slide or step through with line highlights — see [code-slides.md](code-slides.md) |
| Layout breaks on the projector | Styles sized in `vw`/`vh` or media queries | Size everything in `px`/`em` for the fixed canvas; reveal.js scales the whole canvas |
| All list items appear at once | Missing `class="fragment"` | Apply the default fragment rule above |
