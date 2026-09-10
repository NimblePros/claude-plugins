# reveal.js Theming and Layout

## Built-in Themes

reveal.js 6 ships these themes under `dist/theme/`: `black` (default), `white`, `league`, `beige`, `night`, `serif`, `simple`, `solarized`, `moon`, `dracula`, `sky`, `blood`, and the high-contrast `black-contrast` and `white-contrast`.

```html
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/reveal.js@6.0.2/dist/theme/black.css">
```

Pick the closest built-in theme as a base, then override its variables rather than writing a theme from scratch. For rooms with washed-out projectors, start from a `-contrast` theme.

### Theme variables

Every built-in theme exposes CSS custom properties on `:root`. Overriding them restyles all of reveal.js's own rules at once:

| Variable | Controls |
|----------|----------|
| `--r-background-color` | Slide background |
| `--r-main-font`, `--r-main-font-size`, `--r-main-color` | Body text |
| `--r-heading-font`, `--r-heading-color`, `--r-heading-font-weight`, `--r-heading-text-transform` | Headings |
| `--r-code-font` | Code blocks and inline code |
| `--r-link-color`, `--r-link-color-hover` | Links |
| `--r-selection-background-color`, `--r-selection-color` | Text selection |
| `--r-block-margin`, `--r-heading-margin` | Vertical rhythm |

---

## Shared Theme: Reusing Look & Feel Across Decks

Store all design tokens in a shared `theme.css` (or `styles.css`) using CSS custom properties. Each deck links to the same file; changing the variables changes every presentation.

```css
/* theme.css — shared across all decks */
:root {
  --background-color: #0D1B2A;
  --primary-color:    #56B4D3;
  --accent-color:     #2DD4BF;
  --text-color:       #E8F4F8;
  --muted-color:      #7FB3C8;
  --card-bg:          #122334;
  --card-border:      #1E3A52;
  --heading-font:     "Inter", sans-serif;
  --body-font:        "Inter", sans-serif;
  --code-font:        "JetBrains Mono", monospace;
  --base-font-size:   32px;
  --h1-size:          52pt;
  --h2-size:          34pt;
  --slide-padding:    70px;
  --box-radius:       10px;

  /* Map the tokens onto reveal.js's own variables so built-in rules follow them */
  --r-background-color: var(--background-color);
  --r-main-color:        var(--text-color);
  --r-main-font:         var(--body-font);
  --r-main-font-size:    var(--base-font-size);
  --r-heading-color:     var(--text-color);
  --r-heading-font:      var(--heading-font);
  --r-code-font:         var(--code-font);
  --r-link-color:        var(--primary-color);
}

.reveal h1 { font-size: var(--h1-size); }
.reveal h2 { font-size: var(--h2-size); }
```

Link it **after** the built-in theme so its variables win:

```html
<link rel="stylesheet" href=".../dist/theme/black.css">
<link rel="stylesheet" href="theme.css">
```

**Per-deck overrides:** add a second `<link>` after `theme.css` that overrides only what differs:

```html
<link rel="stylesheet" href="theme.css">
<link rel="stylesheet" href="theme-override.css">  <!-- override --primary-color etc. -->
```

**Web fonts:** load them with a `<link>` to the font provider (or `@font-face` with local files) before `theme.css`. If the talk may be given offline, bundle the font files next to the deck — a missing font silently falls back and reflows every slide.

---

## Layout Helpers

reveal.js provides layout classes that work on the fixed canvas:

| Class | Effect |
|-------|--------|
| `r-stack` | Stacks children on top of each other — combine with fragments to swap images in place |
| `r-fit-text` | Scales text to fill the slide width — for one-word or one-line statement slides |
| `r-stretch` | Makes one element (image, video, code block) fill the remaining vertical space |
| `r-hstack` / `r-vstack` | Horizontal / vertical flex row with centered children |

```html
<!-- Swap three diagrams in place, one per click -->
<div class="r-stack">
  <img class="fragment" src="step-1.svg" alt="Monolith">
  <img class="fragment" src="step-2.svg" alt="Monolith with extracted auth service">
  <img class="fragment" src="step-3.svg" alt="Three services behind a gateway">
</div>

<!-- Statement slide -->
<section>
  <h2 class="r-fit-text">It's always DNS.</h2>
</section>

<!-- Screenshot that fills the space under the heading -->
<section>
  <h2>The dashboard at 3 a.m.</h2>
  <img class="r-stretch" src="dashboard.png" alt="Error rate spiking to 40%">
</section>
```

Only one `r-stretch` element per slide, and it must be a direct child of the `<section>`.
