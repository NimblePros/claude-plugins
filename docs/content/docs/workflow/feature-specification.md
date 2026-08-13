---
title: "feature-specification"
description: "The structured feature-spec format: phased checklists with verifiable Done When criteria"
summary: "Specs a developer can implement without asking questions"
date: 2026-08-13
weight: 10
---

> Generates a structured feature specification markdown file from a GitHub issue. Use when
> creating a spec for a new feature, epic, or backlog item that needs implementation planning.

## What it covers

The exact shape of a spec file written to `.claude/specs/<issue-number>-<short-title>.md`:

- A header block — title, `**GitHub Issue**: #<number>`, status, Overview, Acceptance Criteria,
  Out of Scope, Open Questions.
- Sequential implementation phases, each with a Goal, a `- [ ]` task checklist, Details, and
  objectively verifiable **"Done When"** exit conditions — subjective criteria are explicitly
  banned.
- Each phase header names the agent or role that executes it, falling back to a plain role
  ("backend", "UI") when the project has no matching agent.
- Opinionated phase ordering: data layer → API layer → UI layer → tests.
- Every API endpoint gets example request/response JSON; ambiguity means asking clarifying
  questions, not assuming.

## Cross-references

This is the document format the `spec-agent` delegates to — the agent decides *when* to write a
spec; this skill defines *what one looks like*. Downstream, the `pr-agent` verifies the spec's
acceptance criteria before opening a PR. The sibling
[implement-feature]({{< ref "implement-feature" >}}) skill uses a similar phased structure but
writes *plans* to `.claude/plans/` instead. Not slash-invocable; no supporting files.
