# PDF Extraction Reference

When a design asset is supplied as a PDF, each page must be extracted as a separate reference image before comparison.

## Option 1 — Using `pdftoppm` (cross-platform via poppler)

```bash
# Install poppler (macOS)
brew install poppler

# Install poppler (Ubuntu/Debian)
sudo apt-get install -y poppler-utils

# Extract all pages as 150 DPI PNG images
pdftoppm -r 150 -png design.pdf design-page

# Output: design-page-1.png, design-page-2.png, ...
```

Use 150 DPI for standard comparison. Use 300 DPI if the design includes fine typography details.

## Option 2 — Using `pdfimages` for embedded rasters

If the PDF contains rasterized pages (e.g., exported Figma frames):

```bash
pdfimages -all design.pdf design-img
```

## Option 3 — Using Python (pdf2image / PyMuPDF)

```python
import fitz  # PyMuPDF

doc = fitz.open("design.pdf")
for i, page in enumerate(doc):
    pix = page.get_pixmap(dpi=150)
    pix.save(f"design-page-{i + 1}.png")
```

## After Extraction

- Name files consistently: `design-page-<N>.png` where N is 1-based.
- Record the intended viewport width from the design (check file metadata or ask the user).
- Map each extracted page to its corresponding app route before proceeding to Step 2.
