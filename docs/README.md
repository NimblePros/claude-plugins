# NimblePros Claude Plugins Documentation Site

This documentation site uses [Geekdoc](https://github.com/thegeeklab/hugo-geekdoc), a pure Hugo
theme with no npm dependencies. It mirrors the setup used by the
[nmbl](https://github.com/NimblePros/nmbl) documentation site.

## Prerequisites

- Hugo Extended 0.112.0 or later, **or** the .NET 10 SDK (the `serve.cs` script downloads a
  pinned Hugo automatically)

## Quick Start

### Run Development Server

From the `docs/` directory:

```bash
dotnet run serve.cs
```

This downloads the Geekdoc theme and a pinned Hugo Extended binary on first run, then serves the
site at <http://localhost:1316>.

If you already have Hugo installed, first download the theme (any of `serve.cs`, `build.sh`, or a
manual extract of the [Geekdoc release tarball](https://github.com/thegeeklab/hugo-geekdoc/releases)
into `themes/hugo-geekdoc/`), then:

```bash
hugo server
```

### Build for Production

```bash
./build.sh
```

`build.sh` downloads pinned versions of Hugo and the theme, then runs `hugo --minify`. Output goes
to `public/`. It is written for Linux CI environments (e.g. Cloudflare Pages); locally, prefer
`serve.cs` or your own Hugo install.

## Project Structure

```
docs/
├── config/           # Hugo configuration
│   ├── _default/
│   │   ├── hugo.toml         # Main Hugo config
│   │   ├── params.toml       # Theme parameters
│   │   └── menus/
│   │       └── menus.en.toml # Navigation menus
│   └── production/
│       └── hugo.toml         # Production overrides
├── content/          # Markdown content files
│   ├── _index.md          # Documentation hub landing page
│   ├── getting-started/   # Installation and usage
│   ├── dotnet/            # One section per plugin...
│   ├── dotnet-testing/
│   ├── testing/
│   ├── architecture/
│   └── workflow/
├── layouts/          # Hugo template overrides (home page, a11y fixes)
├── static/           # Static files served at site root
│   └── css/
│       └── custom.css       # ⬅ ALL CSS CHANGES GO HERE
├── build.sh          # CI/production build script
└── serve.cs          # Development server script (.NET 10 file-based app)
```

## Theme Customization

Custom styling is in `static/css/custom.css`. This overrides Geekdoc defaults to match NimblePros
brand colors and includes WCAG contrast fixes.

The file is injected via `layouts/partials/head/custom.html`, which overrides Geekdoc's extension
point. Do **not** use `assets/css/` or the `geekdocCustomCSS` param — neither is functional with
this version of Geekdoc.

## Adding Content

Documentation pages summarize the plugins' skills, agents, and commands — they intentionally do
**not** duplicate `SKILL.md` content. When a plugin gains or loses a skill, agent, or command,
update the matching page under `content/<plugin>/`.

Frontmatter for a new page:

```yaml
---
title: "Page Title"
description: "Page description"
weight: 10
---
```

Pages are added to navigation automatically based on directory structure and `weight` (lower
first).

## Learn More

- [Hugo Documentation](https://gohugo.io/documentation/)
- [Geekdoc Documentation](https://geekdocs.de/)
