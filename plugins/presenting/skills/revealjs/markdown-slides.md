# Authoring reveal.js Slides in Markdown

Markdown slides need the Markdown plugin:

```js
import Reveal from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/reveal.mjs';
import RevealMarkdown from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/markdown.mjs';

Reveal.initialize({
  plugins: [RevealMarkdown],
  markdown: { smartypants: true },
});
```

---

## Inline Markdown on a Single Slide

```html
<section data-markdown>
  <textarea data-template>
## Slide Title
- Point one
- Point two
  </textarea>
</section>
```

Inline Markdown works from `file://`. Keep the Markdown flush-left inside the `<textarea>` — leading indentation turns lines into code blocks.

---

## Entire Deck from a `.md` File

```html
<section data-markdown="slides.md"
         data-separator="^\n---\n"
         data-separator-vertical="^\n--\n"
         data-separator-notes="^Notes:">
</section>
```

`slides.md` structure:

```markdown
## Slide 1
Content here

---

## Slide 2
Content here

Notes:
Speaker notes go here

--

## Vertical sub-slide
```

**External Markdown must be served over HTTP.** Browsers block `fetch` from `file://`, so the deck shows an empty slide. Serve the folder instead:

```bash
npx serve .              # or
python -m http.server 8000
```

If the deck must work as a single file opened from disk, use inline `<textarea data-template>` slides or plain HTML.

---

## Slide and Element Attributes

HTML comments attach attributes that Markdown can't express.

**Slide attributes** — backgrounds, transitions, auto-animate:

```markdown
<!-- .slide: data-background-color="#0A1520" data-transition="zoom" -->
## Part Two
```

**Element attributes** — fragments, classes, styles on the preceding element:

```markdown
- First point <!-- .element: class="fragment" -->
- Second point <!-- .element: class="fragment" -->
- Third point <!-- .element: class="fragment" -->
```

The default fragment rule (3+ items → one fragment per item) applies to Markdown slides too.

---

## Code in Markdown Slides

Fenced code blocks get syntax highlighting (with the Highlight plugin) and accept line-highlight steps in brackets after the language:

````markdown
```js [1|3-4|6]
const cache = new Map();

function get(key) {
  if (cache.has(key)) return cache.get(key);
  const value = load(key);
  cache.set(key, value);
  return value;
}
```
````

Each `|`-separated group is one click. See [code-slides.md](code-slides.md) for the HTML equivalents.
