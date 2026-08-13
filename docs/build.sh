#!/usr/bin/env bash
set -euo pipefail

HUGO_VERSION="0.161.1"
GEEKDOC_VERSION="v4.1.0"
THEME_DIR="themes/hugo-geekdoc"

# Cloudflare Pages does not reliably honour HUGO_VERSION; download explicitly.
echo "Downloading Hugo v${HUGO_VERSION}..."
curl -sL "https://github.com/gohugoio/hugo/releases/download/v${HUGO_VERSION}/hugo_extended_${HUGO_VERSION}_linux-amd64.tar.gz" \
  | tar xz hugo

if [ ! -d "$THEME_DIR" ]; then
  echo "Downloading Geekdoc theme ${GEEKDOC_VERSION}..."
  mkdir -p "$THEME_DIR"
  curl -sL "https://github.com/thegeeklab/hugo-geekdoc/releases/download/${GEEKDOC_VERSION}/hugo-geekdoc.tar.gz" \
    | tar xz --strip-components=1 -C "$THEME_DIR"
fi

echo "Building site..."
./hugo --minify
