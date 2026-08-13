# architecture

Architecture decisions and the documentation that records them. Language-agnostic by design —
architecture philosophy does not live inside a language plugin.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install architecture@nimblepros
```

## Skills

| Skill | What it covers |
|---|---|
| `adr-writing` | ADR file naming, frontmatter, required sections, status lifecycle, index maintenance, and detecting/resolving duplicate ADR numbers from parallel branches. Ships with a template and worked examples. |

## Agents

| Agent | Purpose |
|---|---|
| `technical-writer` | Creates and reviews API docs, READMEs, guides, ADRs, and changelogs for clarity and accuracy. |

## Roadmap

Candidates for this plugin as content is written: `modular-monolith`, `vertical-slice`,
`clean-architecture`, `architecture-review`, and a `dependency-analyzer` agent.
