#!/usr/bin/env python3
"""Structural validation for this marketplace repository.

Run from the repo root:

    python tests/validate_marketplace.py

Checks that the marketplace catalog, the plugin manifests, and the skill/agent/command
frontmatter all agree with each other. Exits non-zero and prints every failure if anything is
inconsistent. Standard library only — no dependencies to install.
"""

from __future__ import annotations

import json
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parent.parent
ERRORS: list[str] = []


def fail(message: str) -> None:
    ERRORS.append(message)


def rel(path: Path) -> str:
    return path.relative_to(ROOT).as_posix()


def load_json(path: Path) -> dict | None:
    try:
        return json.loads(path.read_text(encoding="utf-8"))
    except (OSError, json.JSONDecodeError) as exc:
        fail(f"{rel(path)}: not readable as JSON ({exc})")
        return None


def frontmatter(path: Path) -> dict[str, str] | None:
    """Parse the top-level `key: value` pairs of a markdown file's YAML frontmatter."""
    lines = path.read_text(encoding="utf-8").splitlines()
    if not lines or lines[0].strip() != "---":
        fail(f"{rel(path)}: missing YAML frontmatter (no opening ---)")
        return None
    fields: dict[str, str] = {}
    for line in lines[1:]:
        if line.strip() == "---":
            return fields
        if line[:1] in (" ", "\t", "#") or ":" not in line:
            continue  # nested value or comment; only top-level keys matter here
        key, _, value = line.partition(":")
        fields[key.strip()] = value.strip().strip("'\"")
    fail(f"{rel(path)}: missing YAML frontmatter (no closing ---)")
    return None


def check_markdown(path: Path, required: list[str], expect_name_match: bool) -> None:
    fields = frontmatter(path)
    if fields is None:
        return
    for key in required:
        if not fields.get(key):
            fail(f"{rel(path)}: frontmatter is missing a non-empty `{key}`")
    if expect_name_match and fields.get("name"):
        expected = path.parent.name if path.name == "SKILL.md" else path.stem
        if fields["name"] != expected:
            fail(
                f"{rel(path)}: frontmatter name `{fields['name']}` does not match "
                f"its location (`{expected}`)"
            )


def main() -> int:
    # 1. The marketplace catalog.
    catalog_path = ROOT / ".claude-plugin" / "marketplace.json"
    if not catalog_path.is_file():
        fail(".claude-plugin/marketplace.json is missing")
        return report()
    catalog = load_json(catalog_path)
    if catalog is None:
        return report()

    if (ROOT / ".claude-plugin" / "plugin.json").exists():
        fail(
            ".claude-plugin/plugin.json exists — no plugin is rooted at the repo any more; "
            "plugins live under plugins/<name>/ (see ADR 0001)"
        )

    catalogued: set[str] = set()
    for entry in catalog.get("plugins", []):
        name, source = entry.get("name"), entry.get("source")
        if not name or not source:
            fail(f"marketplace.json: plugin entry missing name or source: {entry!r}")
            continue
        catalogued.add(name)
        plugin_dir = (ROOT / source).resolve()
        if not plugin_dir.is_dir():
            fail(f"marketplace.json: `{name}` source `{source}` is not a directory")
            continue
        manifest_path = plugin_dir / ".claude-plugin" / "plugin.json"
        if not manifest_path.is_file():
            fail(f"{source}: missing .claude-plugin/plugin.json")
            continue
        manifest = load_json(manifest_path)
        if manifest is None:
            continue
        if manifest.get("name") != name:
            fail(
                f"{rel(manifest_path)}: name `{manifest.get('name')}` does not match the "
                f"marketplace entry `{name}`"
            )
        if not manifest.get("version"):
            fail(f"{rel(manifest_path)}: missing `version`")
        if not manifest.get("description"):
            fail(f"{rel(manifest_path)}: missing `description`")

    # 2. Every plugin directory is catalogued.
    plugins_dir = ROOT / "plugins"
    if not plugins_dir.is_dir():
        fail("plugins/ directory is missing")
        return report()
    on_disk = {p.name for p in plugins_dir.iterdir() if p.is_dir()}
    for orphan in sorted(on_disk - catalogued):
        fail(f"plugins/{orphan}: not listed in .claude-plugin/marketplace.json")

    # 3. Nothing is left at the repo root.
    for stale in ("skills", "agents", "commands"):
        if (ROOT / stale).exists():
            fail(f"{stale}/ still exists at the repo root; it belongs under plugins/<name>/")

    # 4. Skill, agent, and command frontmatter.
    counts = {"skills": 0, "agents": 0, "commands": 0}
    for plugin_dir in sorted(p for p in plugins_dir.iterdir() if p.is_dir()):
        for skill_dir in sorted((plugin_dir / "skills").glob("*/")):
            skill_file = skill_dir / "SKILL.md"
            if not skill_file.is_file():
                fail(f"{rel(skill_dir)}: skill folder has no SKILL.md")
                continue
            check_markdown(skill_file, ["name", "description"], expect_name_match=True)
            counts["skills"] += 1
        for agent_file in sorted((plugin_dir / "agents").glob("*.md")):
            check_markdown(agent_file, ["name", "description"], expect_name_match=True)
            counts["agents"] += 1
        for command_file in sorted((plugin_dir / "commands").glob("*.md")):
            check_markdown(command_file, ["description"], expect_name_match=False)
            counts["commands"] += 1

    print(
        f"Checked {len(catalogued)} plugins, {counts['skills']} skills, "
        f"{counts['agents']} agents, {counts['commands']} commands."
    )
    return report()


def report() -> int:
    if ERRORS:
        print(f"\n{len(ERRORS)} problem(s) found:\n", file=sys.stderr)
        for error in ERRORS:
            print(f"  - {error}", file=sys.stderr)
        return 1
    print("All structural checks passed.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
