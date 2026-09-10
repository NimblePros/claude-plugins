# Code Slides in reveal.js

Mechanics for showing code with the Highlight plugin. For *what* code to put on a slide and how
much, see the `technical-visuals` skill.

## Setup

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/highlight/monokai.css">
```

```js
import RevealHighlight from 'https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/plugin/highlight.mjs';
Reveal.initialize({ plugins: [RevealHighlight] });
```

`monokai.css` and `zenburn.css` ship with reveal.js. Any [highlight.js theme](https://highlightjs.org/examples) stylesheet also works — pick one whose contrast holds up on a projector.

---

## Basic Code Block

```html
<pre><code class="language-csharp" data-trim data-noescape>
public record Order(Guid Id, decimal Total);
</code></pre>
```

| Attribute | Effect |
|-----------|--------|
| `class="language-xxx"` | Sets the grammar; omit to auto-detect (less reliable) |
| `data-trim` | Strips the leading/trailing whitespace your HTML indentation adds |
| `data-noescape` | Leaves HTML inside the block alone, so you can add `<mark>` highlights by hand |
| `data-line-numbers` | Shows line numbers; with a value, highlights lines (below) |
| `data-ln-start-from="40"` | Starts numbering at 40 — for excerpts from a larger file |

### Escaping `<` and `>`

Code inside HTML must escape `<` as `&lt;` — otherwise `List<Order>` renders as `List` and a stray tag. To paste code verbatim instead, wrap it in a template script:

```html
<pre><code class="language-csharp" data-trim>
<script type="text/template">
var orders = new List<Order>();
if (a < b && c > d) { }
</script>
</code></pre>
```

---

## Step-Through Line Highlights

Instead of shrinking a long snippet, show all of it and walk the audience through it. Each `|`-separated group is one click:

```html
<pre><code class="language-js" data-trim data-line-numbers="1-2|4-6|8">
const cache = new Map();
const ttlMs = 60_000;

function get(key) {
  const hit = cache.get(key);
  if (hit && Date.now() - hit.at < ttlMs) return hit.value;

  return refresh(key);
}
</code></pre>
```

- `data-line-numbers="3,5-7"` highlights a fixed set with no steps.
- Say what the audience should look at *before* each click; the highlight confirms it.
- Keep the steps to 3–5. More than that means the snippet should be two slides.

---

## Morphing Code with Auto-Animate

Consecutive `data-auto-animate` slides with a matching `data-id` on the `<pre>` animate the diff between versions — lines slide into place and new lines fade in. Ideal for "before → after" refactors and building code up line by line.

```html
<section data-auto-animate>
  <pre data-id="code"><code class="language-js" data-trim data-line-numbers>
function hello() {
  console.log("Hello");
}
  </code></pre>
</section>

<section data-auto-animate>
  <pre data-id="code"><code class="language-js" data-trim data-line-numbers="1-3">
function hello(name) {
  const msg = `Hello, ${name}!`;
  console.log(msg);
}
  </code></pre>
</section>
```

---

## Sizing and Layout

- Built-in themes cap `pre code` at `max-height: 400px` with `overflow: auto`, so a longer block shows a **scrollbar** on the projector. Either put `class="r-stretch"` on the `<pre>` (removes the cap and fills the space under the heading) or raise the cap in the theme.
- Set the code font size once in the theme rather than per slide, so every code slide reads the same. Themes default to `.55em` of the main font (~23px); don't go below ~20px on a 1280×720 canvas:

  ```css
  .reveal pre { font-size: 24px; line-height: 1.35; }
  .reveal pre code { max-height: 560px; }
  ```

- The default `pre` width is 90% of the slide; for side-by-side code, put two `<pre>` elements in a `r-hstack` or a two-column grid and cut each to ~12 lines.

## Terminal Output

Use a plain block with a shell-ish grammar and trim it to the lines that matter:

```html
<pre><code class="language-bash" data-trim>
$ dotnet test
  Passed!  - Failed: 0, Passed: 142, Skipped: 0
</code></pre>
```

Replace elided output with a single `…` line rather than showing a scrollbar.
