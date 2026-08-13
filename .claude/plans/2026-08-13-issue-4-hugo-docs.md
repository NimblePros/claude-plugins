# Issue 4 — Add Hugo Docs

**GitHub Issue**: #4
**Status**: Implemented — PR pending merge

## Overview

Add a `/docs` folder containing a Hugo documentation site for the marketplace, using the sibling
`nmbl` repository's docs site as the template/reference (same Geekdoc theme, same config layout,
same custom layouts and CSS). The content is a hierarchy: marketplace overview → one section per
plugin → one summary page per skill (plus a page for the `/add-coverage` command). Per the issue,
pages summarize rather than duplicate the skills, and call out where skills cross-reference
agents, commands, and other skills.

## Phases

### Phase 1: Site scaffolding — done

- [x] `docs/config/` (default + production) adapted from nmbl: Geekdoc theme, search, dark mode,
      breadcrumbs, edit-this-page pointing at `NimblePros/claude-plugins`
- [x] Layout overrides copied from nmbl (custom home page, accessibility fixes in `baseof.html`,
      `menu.html`, `site-footer.html`, table wrapping in `utils/content.html`)
- [x] `static/css/custom.css` copied (NimblePros brand colors + WCAG contrast fixes)
- [x] `build.sh` (CI build, pinned Hugo 0.161.1 + Geekdoc v4.1.0) and `serve.cs` (dev server on
      port 1316, auto-downloads theme and Hugo) adapted from nmbl
- [x] `docs/.gitignore` excludes `public/`, `themes/`, `.hugo-bin/`
- [x] No logo/favicon assets carried over — nmbl's are nmbl-branded; Geekdoc defaults used until
      NimblePros art is added

### Phase 2: Content — done

- [x] Home page (`content/_index.md`) and marketplace overview (`content/docs/_index.md`)
      explaining the capability-based five-plugin split, linking ADR 0001
- [x] Getting Started: the three install options, namespacing, migration from the retired
      single `nimblepros` plugin
- [x] One section per plugin (`dotnet`, `dotnet-testing`, `testing`, `architecture`, `workflow`)
      with skills table, agent summaries, requirements, and related-plugin pointers
- [x] One summary page per skill (19 total) with frontmatter-description quote, "What it covers"
      summary, supporting-files table where applicable, and a Cross-references section
- [x] Dedicated page for `/dotnet-testing:add-coverage` covering the six-phase pipeline and the
      handoff files
- [x] Cross-reference notes derived from reading the actual SKILL.md/agent files (e.g. which
      agents load which skills, the two PR-creation paths in `workflow`, spec vs. plan
      directories)

### Phase 3: Verification — done

- [x] `hugo --minify` builds clean (43 pages, no warnings; `languageCode` deprecation fixed by
      switching to `locale`)
- [x] All internal links use `ref` shortcodes, so broken links fail the build
- [x] `dotnet run scripts/ValidateMarketplace.cs` still passes

## Deferred / not in scope

- Deployment wiring (Cloudflare Pages/GitHub Pages). `build.sh` is ready for a Pages-style host;
  hooking up hosting is a separate decision.
- NimblePros logo/favicon assets for the site header.
- A CI job that runs `hugo --minify` on PRs touching `docs/`.
