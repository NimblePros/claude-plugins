# reveal.js: Animations, Transitions & Reference

## Slide Transitions

Set globally or per-slide with `data-transition`:

```js
Reveal.initialize({ transition: 'slide' });  // default for all slides
```

```html
<section data-transition="zoom">...</section>
<section data-transition="fade-in slide-out">...</section>  <!-- in/out independently -->
```

| Value | Effect |
|-------|--------|
| `none` | Instant cut |
| `fade` | Cross-fade |
| `slide` | Push from right (default) |
| `convex` | Slight 3D convex |
| `concave` | Slight 3D concave |
| `zoom` | Scale in/out |

Speed: `data-transition-speed="fast"` / `"slow"` (default: `"default"`).

Each side of a transition can differ — useful for "chapter break" slides:

```html
<!-- Dramatic zoom in, then normal slide out -->
<section class="section-divider" data-transition="zoom-in slide-out">
  <h1>Part 3: The Dark Side</h1>
</section>
```

### Background transitions

```html
<section data-background="#1E3A52" data-background-transition="zoom">
<section data-background-image="photo.jpg" data-background-opacity="0.25">
<section data-background-video="intro.mp4" data-background-video-muted>
<section data-background-gradient="linear-gradient(to bottom, #0D1B2A, #1E3A52)">
```

---

## Fragment Styles

The base class is `fragment` (fade in); add a style class for a different effect.

| Class | Effect |
|-------|--------|
| `fragment` (default) | Fade in |
| `fade-out` | Visible first, fades out |
| `fade-up` / `fade-down` / `fade-left` / `fade-right` | Slide in from a direction while fading in |
| `fade-in-then-out` | Appears, then disappears on the next step |
| `fade-in-then-semi-out` | Appears, then dims to 50% on the next step |
| `semi-fade-out` | Visible first, dims to 50% |
| `current-visible` | Visible only while it is the current fragment |
| `grow` / `shrink` | Scale up / down |
| `zoom-in` | Zooms in from small |
| `strike` | Strikethrough |
| `highlight-red` / `highlight-green` / `highlight-blue` | Turns the text that color and keeps it |
| `highlight-current-red` / `-green` / `-blue` | Colors the text only while it is the current fragment |

```html
<ul>
  <li class="fragment">Appears first</li>
  <li class="fragment fade-up">Fades up second</li>
  <li class="fragment highlight-blue">Highlights third</li>
</ul>
```

`fade-in-then-semi-out` is the best default for walking through a list: the current point is bright and the earlier ones recede.

### Ordering

Use `data-fragment-index` to control sequence. Fragments that share an index appear together:

```html
<p class="fragment" data-fragment-index="2">Shows second</p>
<p class="fragment" data-fragment-index="1">Shows first</p>
```

Nest fragments to apply two effects in sequence to one element:

```html
<span class="fragment fade-in">
  <span class="fragment highlight-red">Appears, then turns red</span>
</span>
```

### Staggered fragments

Fragments animate with CSS **transitions**, so stagger them with `transition-delay` (not `animation-delay`). Give the items the same `data-fragment-index` so they reveal on one click and cascade:

```html
<ul class="stagger">
  <li class="fragment fade-up" data-fragment-index="1">Latency</li>
  <li class="fragment fade-up" data-fragment-index="1">Throughput</li>
  <li class="fragment fade-up" data-fragment-index="1">Error rate</li>
</ul>
```

```css
.reveal .stagger .fragment:nth-child(2) { transition-delay: 0.1s; }
.reveal .stagger .fragment:nth-child(3) { transition-delay: 0.2s; }
```

### Custom fragment classes

```css
/* Custom: underline sweep on reveal, text visible from the start */
.reveal .fragment.highlight-underline {
  opacity: 1;
  visibility: inherit;
}
.reveal .fragment.highlight-underline.visible {
  text-decoration: underline;
  text-decoration-color: var(--accent-color);
  text-underline-offset: 4px;
}
```

```html
<p class="fragment highlight-underline">Key term</p>
```

### Fragment events

```js
Reveal.on('fragmentshown', (event) => { /* event.fragment is the element */ });
Reveal.on('fragmenthidden', (event) => { /* ... */ });
```

---

## Auto-Animate

Add `data-auto-animate` to consecutive slides sharing elements — reveal.js smoothly morphs matching elements between them. Elements match by `data-id`, or automatically when they have the same tag and text content.

```html
<!-- Slide A -->
<section data-auto-animate>
  <h2 data-id="title">Services</h2>
  <div data-id="box" style="width:100px; background:#56B4D3; height:4px;"></div>
</section>

<!-- Slide B — same data-id elements animate into new positions -->
<section data-auto-animate>
  <h2 data-id="title" style="font-size: 24pt; color: #7FB3C8;">Services</h2>
  <div data-id="box" style="width:400px; background:#56B4D3; height:4px;"></div>
</section>
```

**Options** (global, or per-slide via `data-auto-animate-duration`, `data-auto-animate-easing`, `data-auto-animate-unmatched`):

```js
Reveal.initialize({
  autoAnimateDuration: 0.8,   // seconds
  autoAnimateEasing: 'ease',  // CSS easing
  autoAnimateUnmatched: true, // fade in elements with no match
});
```

- `data-auto-animate-id="a"` on a slide groups auto-animate runs: slides only animate into each other when their ids match.
- `data-auto-animate-restart` on a slide stops it animating from the previous one.

### Growing diagram elements

```html
<!-- Start: small node -->
<section data-auto-animate>
  <div data-id="svc-a" class="node" style="width:80px; height:80px;"></div>
</section>

<!-- End: full panel with label -->
<section data-auto-animate>
  <div data-id="svc-a" class="node" style="width:200px; height:120px;">
    <p>Auth Service</p>
  </div>
</section>
```

For morphing code blocks between slides, see [code-slides.md](code-slides.md).

### Easing options

```html
<section data-auto-animate data-auto-animate-easing="cubic-bezier(0.77,0,0.175,1)">
```

- `ease` — default, smooth
- `ease-in-out` — slow start and end
- `cubic-bezier(0.25, 0.1, 0.25, 1.0)` — custom
- `linear` — mechanical, good for charts

---

## CSS Keyframe Animations on Slides

reveal.js applies state classes you can target for custom entrance animations:

| Class | When applied |
|-------|-------------|
| `.past` | Slides already seen |
| `.present` | Current slide |
| `.future` | Not yet visited |
| `.visible` | Fragment has been revealed |
| `.current-fragment` | The most recently revealed fragment |

```css
/* Slide-in from bottom on entrance; re-runs each time the slide becomes present */
.reveal section.present .animate-up {
  animation: slideUp 0.6s ease forwards;
}
@keyframes slideUp {
  from { opacity: 0; transform: translateY(40px); }
  to   { opacity: 1; transform: translateY(0); }
}
```

Respect reduced-motion preferences for anything beyond a fade:

```css
@media (prefers-reduced-motion: reduce) {
  .reveal section.present .animate-up { animation: none; opacity: 1; }
}
```

---

## Scroll View

reveal.js 5+ can present the deck as a scrollable page — handy for sharing slides after the talk:

```js
Reveal.initialize({ view: 'scroll', scrollProgress: true });
```

Or append `?view=scroll` to the URL without changing the config. In scroll view, fragments activate as the slide scrolls through the viewport.

---

## Background Parallax

```js
Reveal.initialize({
  backgroundTransition: 'slide',
  parallaxBackgroundImage: 'bg-texture.jpg',
  parallaxBackgroundSize: '2100px 900px',
  parallaxBackgroundHorizontal: 200,
  parallaxBackgroundVertical: 50,
});
```

The background scrolls slower than slides, creating depth.

---

## PDF / Print Export

Open the deck with `?print-pdf` in Chrome or Chromium, then print to PDF (margins: none, background graphics: on):

```text
presentation.html?print-pdf
```

```js
Reveal.initialize({
  pdfSeparateFragments: false,  // one page per slide, not per fragment step
  pdfMaxPagesPerSlide: 1,
  showNotes: 'separate-page',   // optional: speaker notes on their own pages
});
```

Hide presenter-only elements from the export:

```css
@media print {
  .no-print { display: none; }
}
```

---

## Keyboard & Navigation Reference

| Key | Action |
|-----|--------|
| `Space` / `→` | Next slide |
| `Shift+Space` / `←` | Previous slide |
| `↑` / `↓` | Vertical slides |
| `Esc` / `O` | Overview mode |
| `S` | Speaker notes window (Notes plugin) |
| `F` | Fullscreen |
| `B` / `.` | Blackout screen |
| `?` | Keyboard shortcuts overlay |
| `Alt+Click` | Zoom into element (Zoom plugin) |

---

## Plugin Quick Reference

reveal.js 6 plugin modules live at `dist/plugin/<name>.mjs`:

```js
import Reveal          from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/reveal.mjs';
import RevealMarkdown  from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/markdown.mjs';
import RevealHighlight from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/highlight.mjs';
import RevealNotes     from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/notes.mjs';
import RevealMath      from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/math.mjs';
import RevealSearch    from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/search.mjs';
import RevealZoom      from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/zoom.mjs';

Reveal.initialize({
  plugins: [RevealMarkdown, RevealHighlight, RevealNotes, RevealMath.KaTeX, RevealSearch, RevealZoom],
});
```

Name the math plugin `RevealMath`, never `Math` — that shadows the JavaScript `Math` global for the whole module. It offers `RevealMath.KaTeX`, `RevealMath.MathJax2`, `RevealMath.MathJax3`, and `RevealMath.MathJax4`.

Without modules, load `dist/reveal.js` and `dist/plugin/<name>.js` with plain `<script>` tags; they define the globals `Reveal`, `RevealMarkdown`, `RevealHighlight`, `RevealNotes`, `RevealMath`, `RevealSearch`, and `RevealZoom`.

For Highlight, link a theme CSS too:

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/highlight/monokai.css">
```
