---
title: "/design-review command"
description: "Capture Playwright screenshots, diff them against a design asset, and produce a prioritized findings report"
summary: "Full design-QA pipeline from asset to report, with optional GitHub issue creation"
date: 2026-08-14
weight: 50
---

> Compare a design asset (image or PDF) against live Playwright screenshots, produce a
> prioritized findings report, and optionally create GitHub issues per finding.

```bash
/playwright-testing:design-review designs/homepage.png https://localhost:5001
/playwright-testing:design-review designs/checkout.pdf https://staging.example.com/checkout
/playwright-testing:design-review designs/hero.png https://localhost:5001 --viewport 1440x900
/playwright-testing:design-review designs/dashboard.png https://localhost:5001 --focus "sidebar" --create-issues
```

## Arguments

| Argument | Required | Description |
|---|---|---|
| `<design-file>` | Yes | Path to a `.png`, `.jpg`, `.webp`, or `.pdf` design asset |
| `<app-url>` | Yes | URL of the live page to compare against |
| `--viewport WxH` | No | Viewport dimensions (default `1280x800`) |
| `--auth-state <path>` | No | Path to a Playwright storage-state or cookies JSON |
| `--focus "<section>"` | No | UI area to weight more heavily in classification |
| `--create-issues` | No | Create one GitHub issue per `design:critical` / `design:high` finding; prompts before lower severities |

## The pipeline

| Phase | What happens |
|---|---|
| 0. Prerequisites | Verifies `dotnet`; confirms design file exists; checks `gh auth` when `--create-issues` is set |
| 1. Prepare design reference | Copies image or extracts PDF pages to `.claude/design-review-work/design-page-*.png` |
| 2. Capture screenshots | Navigates to the URL with Playwright at the specified viewport; waits for `networkidle`; saves `impl-page-*.png` |
| 3. Visual comparison | Resizes and diffs each pair; produces annotated diff overlays and per-region crops |
| 4. Classify and prioritize | Assigns `design:critical` → `design:info` severity per finding |
| 5. Generate report | Populates the findings-report template; prints the summary table and top findings to the terminal |
| 6. Create issues (optional) | Searches for duplicates first; creates issues for critical/high findings; prompts for lower severities |

## Severity model

| Label | Criteria |
|---|---|
| `design:critical` | Layout broken, content obscured, inaccessible element |
| `design:high` | Wrong component, wrong brand color or typography |
| `design:medium` | Spacing off >8px, missing icon, text truncation |
| `design:low` | Minor shade variation, pixel shadow, icon size ±2px |
| `design:info` | Ambiguous region, dynamic content, out-of-scope variant |

## What it leans on

Built on the [`playwright-design-review`]({{< ref "playwright-design-review" >}}) skill. That
skill defines the severity model, the PDF extraction steps, and the findings-report and
GitHub-issue templates. This command applies the full pipeline automatically from a single
invocation.

## Handoff files

All artifacts are written to `.claude/design-review-work/` and kept after the run as an audit trail:

| Pattern | Contents |
|---|---|
| `design-page-<N>.png` | Extracted design reference page(s) |
| `impl-page-<N>.png` | Playwright screenshot(s) |
| `diff-overlay-<N>-<M>.png` | Annotated diff overlay per region |
| `design-crop-<N>-<M>.png` / `impl-crop-<N>-<M>.png` | Cropped evidence pairs per finding |
| `findings-report.md` | Prioritized findings with summary table |
| `issue-<finding-id>.md` | Per-finding GitHub issue body (when `--create-issues` is set) |
