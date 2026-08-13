# Commit Message Conventions

Follows [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/).

## Format

```text
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

## Types

| Type | Purpose | SemVer impact |
| --- | --- | --- |
| `feat` | New feature | MINOR |
| `fix` | Bug patch | PATCH |
| `build` | Build system or external dependency changes | none |
| `chore` | Maintenance tasks (no production code change) | none |
| `ci` | CI configuration and scripts | none |
| `docs` | Documentation only | none |
| `perf` | Performance improvement | none |
| `refactor` | Code restructure without feature/fix | none |
| `style` | Formatting, whitespace, semicolons (no logic change) | none |
| `test` | Adding or correcting tests | none |

## Scope

Optional. A noun describing the section of the codebase in parentheses: `feat(parser):`, `fix(auth):`.

## Breaking Changes

Indicate with either (or both):

1. `!` after type/scope: `feat!:` or `feat(api)!:`
2. Footer token: `BREAKING CHANGE: <description>` (must be uppercase)

Breaking changes map to a MAJOR SemVer bump and can appear on commits of any type.

`BREAKING-CHANGE` (hyphen) is synonymous with `BREAKING CHANGE` (space) in footers.

## Body

One blank line after the description. Free-form. Explain the *why*, not the *what*.

## Footers

One blank line after body. Format: `token: value` or `token #value`.
Tokens use hyphens instead of spaces (e.g., `Reviewed-by`, `Refs`).
`BREAKING CHANGE` is the only token that may contain a space.

## Examples

```text
feat(lang): add Polish language support
```

```text
fix: prevent racing of requests

Introduce a request id and a reference to the latest request. Dismiss
incoming responses other than the latest request.

Reviewed-by: Z
Refs: #123
```

```text
feat(api)!: remove deprecated endpoints

BREAKING CHANGE: The /v1/users endpoint has been removed. Use /v2/users.
```

```text
chore!: drop support for Node 6

BREAKING CHANGE: requires Node 12 or higher.
```

## Anti-patterns

- No file lists (git show does this)
- No bullet dumps listing every change
- No subject restatement in body
- No self-reference ("This commit...", "This change...")
- No passive voice
