---
description: Compare a design asset (image or PDF) against live Playwright screenshots, produce a prioritized findings report, and optionally create GitHub issues per finding.
argument-hint: <path-to-design-asset> <app-url> [--create-issues]
allowed-tools: Bash(dotnet *), Bash(git *), Bash(gh *), Bash(pwsh *), Bash(pdftoppm *), Bash(python *), Write, Edit, Read, Glob
---

<objective>
Run the full `playwright-design-review` skill pipeline:

1. Accept the design asset and target URL from `$ARGUMENTS`.
2. Capture live Playwright screenshots of the implementation at the specified URL.
3. Perform a visual comparison and classify each difference by severity.
4. Output a prioritized findings report.
5. If `--create-issues` is present in `$ARGUMENTS`, create one GitHub issue per `design:critical` or `design:high` finding (with confirmation for lower severities).

`$ARGUMENTS` format: `<design-file-path> <app-url> [--viewport WxH] [--auth-state <cookies.json>] [--focus "<section>"] [--create-issues]`

Examples:
- `designs/homepage.png https://localhost:5001`
- `designs/checkout.pdf https://staging.example.com/checkout --viewport 1440x900 --create-issues`
</objective>

<context>
Current branch: !`git branch --show-current`
Repo: !`gh repo view --json nameWithOwner -q .nameWithOwner`
</context>

<process>

## Phase 0 — Verify prerequisites

### 0.1 Parse arguments

Extract from `$ARGUMENTS`:

- `DESIGN_FILE` — path to the design asset (required).
- `APP_URL` — target URL (required).
- `VIEWPORT` — from `--viewport WxH`, default `1280x800`.
- `AUTH_STATE` — from `--auth-state <path>`, optional.
- `FOCUS_AREA` — from `--focus "<section>"`, optional.
- `CREATE_ISSUES` — boolean flag, true if `--create-issues` is present.

If `DESIGN_FILE` or `APP_URL` is missing, print usage and stop.

### 0.2 CLI tools

```bash
dotnet --version
```

Confirm the design file exists and is a supported type (`.png`, `.jpg`, `.jpeg`, `.webp`, `.pdf`). Stop if it doesn't exist or has an unsupported extension.

If `CREATE_ISSUES` is true:
```bash
gh auth status
```
Warn if auth fails — issue creation will fail but the rest of the pipeline can continue.

### 0.3 Playwright availability

Identify the E2E test project (glob `**/*.EndToEndTests.csproj` or `**/*E2E*.csproj`). If found, ensure browsers are installed:

```bash
dotnet build <EndToEndTests.csproj>
pwsh <EndToEndTests>/bin/Debug/net*/playwright.ps1 install --with-deps chromium
```

If no E2E project exists, use the standalone Playwright Node.js runner from the `playwright-design-review` skill's Step 2 script.

---

## Phase 1 — Prepare design reference

Create the working directory:

```bash
New-Item -ItemType Directory -Force .claude/design-review-work | Out-Null
```

If `DESIGN_FILE` is a PDF, extract each page as a PNG following [skills/playwright-design-review/references/PDF_EXTRACTION.md](../skills/playwright-design-review/references/PDF_EXTRACTION.md):

```bash
pdftoppm -r 150 -png "$DESIGN_FILE" .claude/design-review-work/design-page
```

If `pdftoppm` is unavailable, fall back to the Python/PyMuPDF method in that reference. List extracted pages and confirm the count with the user before proceeding.

If `DESIGN_FILE` is an image, copy it to `.claude/design-review-work/design-page-1.png`.

---

## Phase 2 — Capture implementation screenshots

For each design page, navigate to the corresponding route and take a full-page screenshot. Use the Playwright script from the `playwright-design-review` skill's Step 2 as a template.

Parse `VIEWPORT` into width and height. Apply `AUTH_STATE` cookies if provided.

Save screenshots to `.claude/design-review-work/impl-page-<N>.png`.

Wait for `networkidle` before capturing. If the page has visible animations or carousels, add a 500ms settling wait.

---

## Phase 3 — Visual comparison

For each page pair (design vs. implementation):

1. Resize the implementation screenshot to match the design reference dimensions.
2. Perform a pixel-level diff or AI-assisted visual analysis.
3. Identify regions with meaningful differences (ignore sub-2px anti-aliasing noise).
4. If `FOCUS_AREA` was specified, weight findings in that section higher.
5. For each region, produce:
   - A cropped design excerpt: `.claude/design-review-work/design-crop-<N>-<M>.png`
   - A cropped implementation excerpt: `.claude/design-review-work/impl-crop-<N>-<M>.png`
   - An annotated diff overlay: `.claude/design-review-work/diff-overlay-<N>-<M>.png`

---

## Phase 4 — Classify and prioritize

Assign each finding a severity using the table from the `playwright-design-review` skill:

| Severity | Label | Criteria |
|----------|-------|----------|
| Critical | `design:critical` | Layout broken, content obscured, inaccessible element |
| High | `design:high` | Wrong component, wrong brand color/typography |
| Medium | `design:medium` | Spacing off >8px, missing icon, text truncation |
| Low | `design:low` | Minor shade variation, pixel-level shadow, icon size ±2px |
| Info | `design:info` | Ambiguous region, dynamic content, out-of-scope variant |

Do not escalate findings based on subjective preference alone.

---

## Phase 5 — Generate findings report

Populate the template at `skills/playwright-design-review/templates/findings-report.md` with all findings (highest severity first).

Write the completed report to `.claude/design-review-work/findings-report.md`.

Print the summary table and the top 3 findings to the terminal.

---

## Phase 6 — Create GitHub issues (conditional)

Only run this phase if `CREATE_ISSUES` is true.

For each `design:critical` or `design:high` finding:

1. Search for an existing open issue to avoid duplicates:

```bash
gh issue list --search "<component keyword>" --state open --json number,title
```

2. If no duplicate exists, populate the template at `skills/playwright-design-review/templates/github-issue-template.md` and write it to `.claude/design-review-work/issue-<finding-id>.md`.

3. Create the issue:

```bash
gh issue create \
  --title "<Component/Page>: <one-line description>" \
  --body-file .claude/design-review-work/issue-<finding-id>.md \
  --label "design,design:critical"
```

For `design:medium` and lower findings, list them and ask the user whether to create issues before proceeding.

---

## Phase 7 — Report

Print to the terminal:

- Full path to `.claude/design-review-work/findings-report.md`.
- Count of findings per severity.
- List of created issue URLs (if Phase 6 ran).
- Inconclusive regions and why they were skipped.

Do not delete `.claude/design-review-work/` — it is the audit trail for this review.

</process>
