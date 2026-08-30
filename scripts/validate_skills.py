#!/usr/bin/env python3
"""Validate every SKILL.md in this repository against the Agent Skills spec.

Wraps the reference validator from https://github.com/agentskills/agentskills
(`skills-ref`), which must already be installed:

    pip install "git+https://github.com/agentskills/agentskills.git#subdirectory=skills-ref"

    python scripts/validate_skills.py [root]

`skills-ref validate` only accepts one skill at a time and knows nothing about
Claude Code's own frontmatter extensions, so this script walks every skill under
plugins/, allows the Claude Code-specific fields listed below, and prints GitHub
Actions annotations for anything that is still wrong. Exits non-zero if any skill
fails.

This validates the *skill* contract. `claude plugin validate` validates the
plugin/marketplace manifests, and scripts/ValidateMarketplace.cs validates how
the two fit together.
"""

import os
import sys
from pathlib import Path

from skills_ref import validator
from skills_ref.errors import ParseError
from skills_ref.parser import find_skill_md, parse_frontmatter

# Frontmatter keys that Claude Code documents as its own extensions to the Agent
# Skills spec (https://code.claude.com/docs/en/skills, "Frontmatter reference").
# Keeping them out of skills-ref's error list lets the rest of its checks
# (required fields, name/directory match, description length) still gate CI.
# Anything outside this set and the spec's six fields is either a typo or a key
# Claude Code silently ignores at load time — both are worth failing on, so add
# to this list only for fields the Claude Code docs actually define.
CLAUDE_CODE_FIELDS = {
    "when_to_use",
    "argument-hint",
    "arguments",
    "disable-model-invocation",
    "user-invocable",
    "disallowed-tools",
    "model",
    "effort",
    "context",
    "agent",
    "background",
    "hooks",
    "paths",
    "shell",
}

SEARCH_ROOT = "plugins"


def find_skills(root: Path) -> list[Path]:
    return sorted(p.parent for p in (root / SEARCH_ROOT).rglob("SKILL.md"))


def validate_skill(skill_dir: Path) -> list[str]:
    """Same checks as `skills-ref validate`, but reading SKILL.md as UTF-8.

    skills_ref.validator.validate() reads with the platform default encoding,
    which mangles non-ASCII skills on Windows.
    """
    skill_md = find_skill_md(skill_dir)
    if skill_md is None:
        return ["Missing required file: SKILL.md"]

    try:
        metadata, _ = parse_frontmatter(skill_md.read_text(encoding="utf-8-sig"))
    except (ParseError, UnicodeDecodeError) as error:
        return [str(error)]

    return validator.validate_metadata(metadata, skill_dir)


def annotate(rel_path: str, message: str) -> None:
    """Emit a GitHub Actions error annotation (a no-op comment when run locally)."""
    escaped = message.replace("%", "%25").replace("\r", "%0D").replace("\n", "%0A")
    print(f"::error file={rel_path}/SKILL.md,line=1::{escaped}")


def main() -> int:
    root = Path(sys.argv[1] if len(sys.argv) > 1 else ".").resolve()
    validator.ALLOWED_FIELDS = validator.ALLOWED_FIELDS | CLAUDE_CODE_FIELDS

    skills = find_skills(root)
    if not skills:
        print(f"No SKILL.md found under {root / SEARCH_ROOT}", file=sys.stderr)
        return 1

    failures: list[tuple[str, list[str]]] = []
    for skill_dir in skills:
        rel = skill_dir.relative_to(root).as_posix()
        errors = validate_skill(skill_dir)
        if errors:
            failures.append((rel, errors))
            annotate(rel, "\n".join(errors))
            print(f"FAIL {rel}")
            for error in errors:
                print(f"       {error}")
        else:
            print(f"  ok {rel}")

    summarize(skills, failures)

    print()
    print(f"{len(skills) - len(failures)}/{len(skills)} skills valid.")
    return 1 if failures else 0


def summarize(skills: list[Path], failures: list[tuple[str, list[str]]]) -> None:
    summary_path = os.environ.get("GITHUB_STEP_SUMMARY")
    if not summary_path:
        return

    lines = ["## Skill validation", ""]
    if failures:
        lines.append(f"{len(failures)} of {len(skills)} skills failed.")
        lines.append("")
        for rel, errors in failures:
            lines.append(f"- `{rel}`")
            lines.extend(f"  - {error}" for error in errors)
    else:
        lines.append(f"All {len(skills)} skills passed.")

    with open(summary_path, "a", encoding="utf-8") as handle:
        handle.write("\n".join(lines) + "\n")


if __name__ == "__main__":
    sys.exit(main())
