---
title: "playwright-design-review"
description: "Compare a design asset against live Playwright screenshots and produce a prioritized findings report"
summary: "From mockup to implementation gap list, with optional GitHub issue creation"
date: 2026-08-14
weight: 20
---

> Compare a design asset (image or PDF) against live screenshots of the implementation captured
> by Playwright. Detects visual differences, prioritizes them by user impact, suggests corrections,
> and optionally creates one GitHub issue per finding.

## What it covers

A structured design-QA pass that produces a human-reviewable audit trail:

1. Accept a reference design as an image (`.png`, `.jpg`, `.webp`) or PDF.
   PDFs are split per-page using `pdftoppm` or PyMuPDF before comparison.
2. Capture full-page Playwright screenshots of the live implementation at the specified viewport.
3. Pixel-diff each pair, annotate a diff overlay, and crop evidence images per region.
4. Classify every difference by severity and output a prioritized findings list.
5. Suggest a concrete, actionable correction per finding.
6. Optionally create one GitHub issue per finding using the `gh` CLI.

The guardrails matter: findings are never escalated to `design:critical` on subjective grounds
alone — functional or accessibility impact is required. Ambiguous or dynamic regions are always
classified as `design:info` rather than guessed.

## Severity model

| Label | Criteria |
|---|---|
| `design:critical` | Core layout broken, content obscured, inaccessible element |
| `design:high` | Wrong component used, wrong brand color or typography affecting readability |
| `design:medium` | Spacing off by more than 8px, missing icon, text truncation |
| `design:low` | Minor shade variation, pixel-level shadow, icon size ±2px |
| `design:info` | Ambiguous region, dynamic content, responsive variant out of scope |

## Ships with

| File | Contents |
|---|---|
| `references/PDF_EXTRACTION.md` | Steps for extracting PDF pages as PNG using `pdftoppm` and PyMuPDF |
| `templates/findings-report.md` | Prioritized findings report with summary table, per-finding evidence, and inconclusive regions |
| `templates/github-issue-template.md` | GitHub issue body with evidence table, bounding-box location, suggested correction, and acceptance criteria |

## Invoke via command

The [`/playwright-testing:design-review`]({{< ref "design-review" >}}) command runs the full
pipeline end-to-end from a single invocation: PDF extraction, Playwright screenshot capture,
visual diff, report generation, and optional GitHub issue creation.

## Cross-references

Pairs with [`playwright-issue-repro-report`]({{< ref "playwright-issue-repro-report" >}}) — that
skill produces evidence from an existing bug report; this skill *generates* the findings before
bugs are reported. Issue creation uses the same `gh` CLI flow as the repro-report skill.
