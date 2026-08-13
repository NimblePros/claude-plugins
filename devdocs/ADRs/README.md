# Architecture Decision Records

These records capture significant structural and technology decisions for this repository. They
are append-only: an ADR that no longer holds is marked `Superseded` and linked to its replacement
rather than edited or deleted.

To add one, copy [0000-YYYY-MM-DD-title.md](0000-YYYY-MM-DD-title.md), name the new file
`NNNN-YYYY-MM-DD-short-slug.md` with the next number, and add a line below in numeric order. The
`adr-writing` skill in `plugins/architecture/skills/adr-writing/` covers the full workflow —
numbering, options format, superseding, and duplicate-number detection.

**Where this repo differs from that skill's template:** the skill's `template.md` is written for
a Hugo-style docs site (`title:`/`weight:` frontmatter, a `Date`/`Status` metadata block, statuses
`Proposed`/`Active`/`Superseded`, an `Options Considered` heading). These ADRs use the plain
heading style of `0000-YYYY-MM-DD-title.md` instead: no frontmatter, `## Status` with `Accepted`,
`## Alternatives Considered`, and the date carried in the filename. The skill itself says existing
repository conventions win, so follow the files here — not the skill's template — when adding an
ADR to *this* repository.

- [ADR 0001 - Publish Multiple Focused Plugins from a Single Marketplace Repository](0001-2026-08-13-multi-plugin-marketplace-structure.md)
