---
name: adr-writing
description: Write Architecture Decision Records (ADRs) for a project's documentation. Covers file naming, frontmatter, required sections, status lifecycle, index maintenance, and detecting/fixing duplicate ADR numbers from parallel branches. Load before creating, revising, or superseding any ADR, or when asked to check for duplicate ADR numbers or fix the ADR list.
user-invocable: false
---

# Writing Architecture Decision Records

## When to Use This Skill

Use this skill when:
- Recording a significant architecture or technology decision (new dependency, service boundary,
  data model change, infrastructure choice, cross-cutting pattern)
- Superseding or amending an existing ADR
- Asked to "write an ADR" or "document this decision"
- Asked to "check for duplicate ADR numbers", "fix the ADR list", or similar index-cleanup requests

Files in this skill folder:

- [template.md](template.md) — blank ADR to copy when creating a new one
- [examples/full-example-adr.md](examples/full-example-adr.md) — a complete worked example showing
  every section, the Options Considered format, and the expected writing style
- [examples/superseded-example-adr.md](examples/superseded-example-adr.md) — a short early-style
  ADR showing the Superseded status line
- [examples/example-index.md](examples/example-index.md) — a sample ADR index file

## Locating the ADR Directory

Before writing anything, find where this repository keeps its ADRs — glob for `**/adr-*.md` or
directories named `adr`/`adrs`. Common locations are `docs/adr/`, `docs/architecture/adrs/`, or a
static-site content directory such as `devdocs/content/architecture/adrs/`. The directory usually
contains an index file (`_index.md`, `index.md`, or `README.md`) listing every ADR.

**Existing repository conventions win over this skill.** Read two or three of the most recent ADRs
first; if their frontmatter, section names, or numbering style differ from what's described here,
match the repository.

If the repository has no ADRs yet, create `docs/architecture/adrs/` with an index file (see
[examples/example-index.md](examples/example-index.md)) and start numbering at 001.

## Creating a New ADR

### 1. Determine the next number

Read the index file and take the highest ADR number listed **in the index titles**, plus one. Do
NOT derive the number from filenames — filenames can drift from titles historically (a file may
have been created with the wrong number and never renamed because renaming changes its published
URL). The index and the `title:` frontmatter are the source of truth.

Before settling on the number, run the duplicate check described in the "Duplicate ADR Numbers"
section below — parallel branches can each pick the same "next" number, and merging both silently
produces two ADRs with one number. Also check open PRs when possible (on GitHub:
`gh pr list --search adr`) so the number you pick isn't already claimed by an in-flight branch; if
it is, take the number after it.

### 2. Create the file

- Path: `<adr-directory>/adr-NNN-short-slug.md`
- `NNN` is the ADR number, zero-padded to 3 digits, and **must match** the number in the title.
- The slug is a short kebab-case summary of the decision (e.g. `outbox-event-publishing`,
  `guid-key-generation`).

Copy [template.md](template.md) and fill it in.

### 3. Frontmatter and metadata

```markdown
---
title: ADR 042 - Outbox Pattern for Order Event Publishing
weight: 42
---

- **Date:** 2026-08-06
- **Status:** Proposed
```

- `title:` is `ADR NNN - <Title in Title Case>`. The title names the decision itself, not the
  problem (e.g. "GUID Keys Generated at Construction", not "ID Generation Problems").
- `weight:` is the ADR number without zero-padding (controls ordering in Hugo-style site nav).
  If the repository's ADRs use different frontmatter — or none — match them instead.
- `Date` is the date the ADR is written, in `YYYY-MM-DD` format.
- `Status` is one of:
  - `Proposed` — decision recorded but not yet implemented/merged
  - `Active` — decision is in effect
  - `[Superseded](<link-to-superseding-adr>)` — replaced; link to the superseding ADR
- Keep metadata to Date and Status; don't invent extra fields (Priority, Deciders, etc.) unless
  the repository already uses them.

### 4. Register it in the index

Add a line to the index file, keeping numeric order:

```markdown
- [ADR 042 - Outbox Pattern for Order Event Publishing](/architecture/adrs/adr-042-outbox-event-publishing/)
```

Use whatever link form the existing index entries use (site path with trailing slash for a
docs-site, relative `.md` link for a plain docs folder). See
[examples/example-index.md](examples/example-index.md).

## Section Structure

Use these `##` sections in this order:

1. **Context** — required
2. **Problem Statement** — recommended; a one-to-three-sentence question the ADR answers
3. **Decision** — required
4. **Options Considered** — required for any decision with real alternatives
5. **Rationale** — recommended
6. **Consequences** — required; split into `### Positive` and `### Negative` when there are both
7. **Open Issues** — optional; genuinely unresolved questions
8. **Future Considerations** — optional; likely follow-on work or revisit triggers
9. **Related Links** — optional; issues, PRs, external docs

[examples/full-example-adr.md](examples/full-example-adr.md) demonstrates all of these filled in.
Short, low-controversy ADRs may omit sections, but new ADRs should follow the full structure
unless the decision is trivially small.

### Options Considered format

Each option gets its own `### Option N: <name>` subsection with a short description of what it
would look like, followed by either:

- `**Rejected because:**` and the concrete reasons, or
- `(chosen)` appended to the heading, with the description explaining what makes it fit.

Rejections must cite real constraints verified in the codebase, not strawmen — e.g. "the library
is not currently a dependency anywhere in the solution (no entry in the package manifest)". See
the Options Considered section of
[examples/full-example-adr.md](examples/full-example-adr.md).

## Writing Style

Match the tone and rigor of [examples/full-example-adr.md](examples/full-example-adr.md):

- **Ground everything in the actual codebase.** Name real classes, files, endpoints, and packages.
  Before writing Context, audit the code so claims like "no event-publishing infrastructure
  exists" are verified, not assumed.
- **Link to the driving issue or PR** when one exists, and to any implementation plan or design
  doc in the repository.
- **Include short code snippets** when the decision is about code shape.
- **Be honest in Negative consequences.** Real costs (indirection, runtime-only failures, ordering
  caveats) belong there, each with its mitigation if one exists. An ADR with no negatives reads as
  unexamined.
- **Wrap prose at roughly 100 characters**, or match the existing ADRs.
- Write in present tense about the decision ("The dispatcher is hand-rolled..."), past tense about
  the investigation ("An audit of the current codebase found...").

## Superseding an ADR

1. Write the new ADR as normal (new number, `Status: Proposed` or `Active`).
2. In the old ADR, change its status line to link to the new one — see
   [examples/superseded-example-adr.md](examples/superseded-example-adr.md).
3. In the new ADR's Context, reference the ADR it replaces and why it no longer holds.
4. Never delete or renumber old ADRs — the record is append-only. (The one exception is resolving
   a duplicate number collision; see below.)

## Duplicate ADR Numbers

Two branches created in parallel can each compute the same "next" number; once both merge, two
ADRs share one number. Check for this **every time a new ADR is created**, and whenever asked to
"check for duplicate ADR numbers", "fix the ADR list", or similar.

### Detecting duplicates

Compare three views of the numbering — they should all agree:

1. **Frontmatter titles** (source of truth): grep `^title: ADR` across the ADR directory and look
   for the same `NNN` appearing twice.
2. **Index entries**: the `ADR NNN` numbers in the index should be a gap-free ascending sequence
   with no repeats, and each entry's link target must be a file that exists.
3. **Filenames**: `adr-NNN-*.md` prefixes should match their frontmatter number. If the repository
   has a long-standing mismatch, leave the published filename alone unless asked to fix it, since
   renaming changes its URL — but note it so it isn't mistaken for a fresh collision.

A duplicate `weight:` value (where the repository uses weights) is the same bug — weights must
match the ADR number, so check those too.

### Resolving a collision

The ADR that merged to the default branch **first** keeps the number. The later one is renumbered
to the next available number (highest existing + 1 — do not fill gaps). Use `git log --follow` or
merge dates to determine which came first; if genuinely ambiguous, keep the number on the ADR that
other documents already reference, since that minimizes link churn.

Renumbering an ADR from `NNN` to `MMM` means updating, atomically in one commit:

1. **The file itself**: rename `adr-NNN-slug.md` → `adr-MMM-slug.md`, update `title:` and
   `weight:` to `MMM`.
2. **The index**: fix the entry's title and URL in the index file and move it into numeric order.
3. **All references across the repo**: grep the whole repository (not just the docs directory) for
   both the path form (`adr-NNN-slug`, which covers markdown links and site URLs) and the prose
   form (`ADR NNN` / `ADR-NNN`) and update every hit that refers to the renumbered ADR — other
   ADRs' Context or Status lines (especially `Superseded` links), docs pages, plan files, README
   or agent-instruction files, and code comments. Prose-form hits need care: confirm each one
   refers to the renumbered ADR and not its same-numbered sibling before editing.
4. **Verify**: re-run the detection pass above and grep for the old `adr-NNN-slug` string to
   confirm nothing still points at the old number/URL. References in merged PR descriptions or
   issue comments can't be fixed retroactively — that's expected; only repository content must be
   consistent.
