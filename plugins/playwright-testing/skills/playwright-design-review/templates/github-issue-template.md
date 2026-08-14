# Design Discrepancy: <Component/Page> — <one-line description>

## Design Review Context

- **Page / Route**: <page-name-or-url>
- **Design Asset**: <design-file-name> (attach or link)
- **Viewport**: <widthxheight>
- **Detected**: <YYYY-MM-DD>
- **Severity**: `design:critical` | `design:high` | `design:medium` | `design:low`

---

## Description

<One or two sentences describing exactly what differs between the design specification and the current implementation.>

---

## Evidence

<!-- Attach or link the relevant cropped images -->

| | Image |
|---|---|
| **Design (expected)** | <!-- attach design-crop.png --> |
| **Implementation (actual)** | <!-- attach impl-crop.png --> |
| **Diff overlay** | <!-- attach diff-overlay.png --> |

---

## Location

- **Section**: <header / hero / navigation / footer / etc.>
- **Approximate coordinates**: (<x1>,<y1>)–(<x2>,<y2>) at <widthxheight> viewport

---

## Suggested Correction

<Concrete, actionable fix. Examples:>

- Change `padding-top` on `.hero-section` from `16px` to `24px`.
- Replace `<Button variant="outline">` with `<Button variant="primary">`.
- Update font-weight from `400` to `600` for the section heading.
- Use the `CheckIcon` component instead of the inline SVG.

---

## Acceptance Criteria

- [ ] Implementation visually matches the design asset at the specified viewport.
- [ ] Responsive variants (if applicable) are also corrected.
- [ ] Designer or product owner has signed off on the fix.

---

## Related

- Design file: <Figma / Zeplin / Sketch link if available>
- Design review report: <link to full findings report if filed>
