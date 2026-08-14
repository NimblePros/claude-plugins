---
name: playwright-design-review
description: Compare a design asset (image or PDF) against live screenshots of the implemented UI. Detects and prioritizes visual differences, suggests corrections, and optionally creates GitHub issues per finding. Use when you want to validate that a delivered implementation matches the intended design.
metadata:
  author: nimblepros
  version: "1.0"
  spec: agentskills.io
---

# Playwright Design Review Skill

## Purpose

Provide a repeatable workflow for:

1. Accepting a design asset (image or PDF) as the visual specification.
2. Capturing live screenshots of the implementation using Playwright.
3. Detecting and categorizing visual differences between design and implementation.
4. Prioritizing findings by user impact and generating actionable correction suggestions.
5. Optionally creating one GitHub issue per finding.

## Use This Skill When

- A designer or product owner has supplied mockups and you want to verify the implementation matches.
- A feature has shipped and you need a structured design-QA pass before release.
- You want a reproducible audit trail of UI deviations for triage.

## Required Inputs

1. Design asset: an image file (`.png`, `.jpg`, `.webp`) or PDF containing the reference design.
   - For PDFs, each page is treated as a separate reference frame.
2. App URL or route to the page(s) being compared.
3. Viewport size used in the design (default: 1280×800 desktop if not specified).
4. Optional: auth state or cookie file needed to reach the page.
5. Optional: list of UI areas to focus on (e.g., header, hero section, navigation).
6. Optional: `--create-issues` flag to file a GitHub issue per finding.

## Workflow

### Step 1 — Prepare the Design Reference

- Accept the design asset provided by the user.
- If the input is a PDF, extract each page as a separate reference image using the steps in [references/PDF_EXTRACTION.md](references/PDF_EXTRACTION.md).
- Crop or annotate specific regions if the user has indicated focus areas.
- Record the canonical viewport and any explicit design notes (colors, spacing, typography).

### Step 2 — Capture Implementation Screenshots

Use Playwright to navigate to each target route and take a full-page screenshot:

```typescript
import { chromium } from '@playwright/test';

const browser = await chromium.launch();
const page = await browser.newPage();
await page.setViewportSize({ width: 1280, height: 800 });

// Apply stored auth state if provided
// await page.context().addCookies([...]);

await page.goto('<target-url>');
await page.waitForLoadState('networkidle');
await page.screenshot({ path: 'screenshots/impl-<route>.png', fullPage: true });

await browser.close();
```

- Use one screenshot per logical view (desktop, tablet, mobile) when responsive variants matter.
- Wait for `networkidle` to avoid capturing loading states.
- If the page has animations, add `await page.waitForTimeout(500)` after load.

### Step 3 — Compare Design vs. Implementation

Perform a structured visual diff:

1. **Resize** the implementation screenshot to match the design reference dimensions before comparison.
2. **Compare** using pixel-level diff or AI-assisted visual analysis.
3. **Annotate** the diff image: highlight regions with differences using colored overlays.
4. **Classify** each difference (see Severity Classification below).

### Step 4 — Severity Classification

| Severity | Label | Criteria |
|----------|-------|----------|
| Critical | `design:critical` | Core layout broken, content obscured, wrong page rendered, inaccessible element |
| High | `design:high` | Wrong component used, wrong color/typography that affects brand or readability |
| Medium | `design:medium` | Spacing/padding off by >8px, missing icon or illustration, text truncation |
| Low | `design:low` | Minor color shade variation, pixel-level shadow difference, icon size ±2px |
| Info | `design:info` | Ambiguous region, placeholder content, responsive variant not in scope |

Assign each finding to exactly one severity level. Do not escalate ambiguous findings — use `design:info`.

### Step 5 — Prioritize and Suggest Corrections

For each finding (highest severity first), produce:

1. **Location**: page name, section, and bounding box coordinates if available.
2. **Description**: one sentence stating what differs from the design.
3. **Severity**: label from the table above.
4. **Correction**: a concrete, actionable suggestion (e.g., change CSS property, swap component, update copy).
5. **Evidence**: paths to cropped diff images for that region.

Output the full prioritized list using the template in [templates/findings-report.md](templates/findings-report.md).

### Step 6 — Create GitHub Issues (optional)

If the user requested `--create-issues`, create one GitHub issue per `design:critical` or `design:high` finding, and optionally for lower severities if the user confirms.

Use the template in [templates/github-issue-template.md](templates/github-issue-template.md) for each issue body.

Post with:

```bash
gh issue create \
  --title "<Component/Page>: <one-line description>" \
  --body-file <issue-file.md> \
  --label "design,<severity-label>"
```

Do not create duplicate issues. Before creating, search existing issues:

```bash
gh issue list --search "<component keyword>" --state open
```

## Output Contract

Return:

1. Side-by-side comparison image (design vs. implementation) or annotated diff.
2. Prioritized findings list (using [templates/findings-report.md](templates/findings-report.md)).
3. Correction suggestions per finding.
4. If `--create-issues` was used: list of created issue URLs, or a dry-run preview if requested.

## Reusable Assets

- PDF extraction steps: [references/PDF_EXTRACTION.md](references/PDF_EXTRACTION.md)
- Findings report template: [templates/findings-report.md](templates/findings-report.md)
- GitHub issue body template: [templates/github-issue-template.md](templates/github-issue-template.md)

## Guardrails

- Do not mark a finding as `design:critical` based on subjective taste alone — require a functional or accessibility impact.
- Do not create GitHub issues without explicit user confirmation unless `--create-issues` was passed up front.
- Do not include PII, credentials, or internal API responses in issue bodies or screenshots.
- Always distinguish confirmed differences from inconclusive regions (e.g., dynamic content, ads, user-generated content).
- If the design asset is ambiguous for a region, classify as `design:info` and note the ambiguity rather than guessing.
