---
description: Write an Architecture Decision Record (ADR) documenting a design or technology decision.
argument-hint: "<decision-description>" [--issue <number>]
allowed-tools: Bash(git *), Bash(gh *), Write, Edit, Read, Glob
---

<objective>
Execute the `adr-writing` skill: locate the ADR directory, determine the next ADR number, draft a new ADR from the decision described in `$ARGUMENTS`, present it for review, then write the file and register it in the index.

`$ARGUMENTS` should describe the decision being recorded (e.g. `"use Outbox pattern for event publishing"`), optionally referencing a GitHub issue with `--issue <N>`.
If omitted, ask the user to describe the decision before proceeding.
</objective>

<context>
Current branch: !`git branch --show-current`
Today's date: !`Get-Date -Format yyyy-MM-dd`
</context>

<process>

## Phase 0 — Gather decision context

If `$ARGUMENTS` is empty, ask:
> "What decision are you recording? Describe the context, the options you considered, and the choice made."

If `--issue <N>` is present in `$ARGUMENTS`, fetch the issue for context:
```bash
gh issue view N --json title,body,labels,comments
```

---

## Phase 1 — Find the ADR directory and conventions

Glob for existing ADRs to locate the ADR directory:
```bash
Get-ChildItem -Recurse -Filter "adr-*.md" | Select-Object -First 5
```

If none are found, ask the user where to place ADRs before continuing.

Read the two most recent ADRs and the index file to learn the repository's conventions (frontmatter fields, section names, numbering style). Existing conventions override the defaults in the `adr-writing` skill.

---

## Phase 2 — Determine the next ADR number

Read the ADR index and find the highest number in the **index titles** — do not derive the number from filenames. Take that number + 1.

Check for in-flight PRs that may have already claimed the same number:
```bash
gh pr list --search "adr"
```

If a conflict exists, take the next available number.

---

## Phase 3 — Draft the ADR

Follow the `adr-writing` skill to fill in the template:
- Title: `ADR NNN - <Title in Title Case>` (names the decision, not the problem)
- Status: `Proposed`
- Date: today's date
- Sections: Context, Decision Drivers, Options Considered, Decision Outcome, Consequences

Present the full draft to the user before writing anything to disk.

---

## Phase 4 — Write and register

After the user approves the draft:
1. Write the file to `<adr-directory>/adr-NNN-<slug>.md`.
2. Add an entry to the index file, keeping entries in numeric order.
3. Confirm the created file path and index entry.

</process>
