# NimblePros Claude Code Plugins

A marketplace of Claude Code plugins authored, curated, and/or edited by the NimblePros team. Add
the marketplace once, then install only the plugins you want.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install dotnet-testing@nimblepros
```

---

## Plugins

| Plugin | Install | What you get |
|---|---|---|
| [**dotnet**](plugins/dotnet/) | `claude plugin install dotnet@nimblepros` | C#/.NET practice: coding standards, package management, Options/DI patterns, OpenTelemetry, NuGet publishing |
| [**dotnet-testing**](plugins/dotnet-testing/) | `claude plugin install dotnet-testing@nimblepros` | The `/dotnet-testing:add-coverage` pipeline, CRAP analysis, Stryker.NET mutation testing, xUnit conventions, Aspire integration testing |
| [**testing**](plugins/testing/) | `claude plugin install testing@nimblepros` | Stack-neutral testing: end-to-end test authoring, Playwright issue-repro reports |
| [**architecture**](plugins/architecture/) | `claude plugin install architecture@nimblepros` | ADR writing and technical documentation |
| [**workflow**](plugins/workflow/) | `claude plugin install workflow@nimblepros` | Issue → spec → implementation → review → PR, over the `gh` CLI |

Each plugin's README documents its skills, agents, commands, and requirements.

---

## Installation

### Option A — Claude Code plugin install (recommended)

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install <plugin>@nimblepros
```

This installs that plugin's commands, agents, and skills into your global Claude Code
configuration so they are available in every project.

### Option B — Per-project install via `.claude/settings.json`

Add the marketplace and the plugins you want to your project's `.claude/settings.json` so all
collaborators get them when they trust the folder:

```json
{
  "extraKnownMarketplaces": {
    "nimblepros": {
      "source": {
        "source": "github",
        "repo": "NimblePros/claude-plugins"
      }
    }
  },
  "enabledPlugins": {
    "dotnet-testing@nimblepros": true,
    "workflow@nimblepros": true
  }
}
```

### Option C — Manual copy (standalone, no marketplace)

Copy a single plugin's contents into your project's `.claude/` directory. Skills and commands are
then available without namespacing (`/add-coverage` instead of `/dotnet-testing:add-coverage`):

```bash
# From your project root
gh repo clone NimblePros/claude-plugins /tmp/nimblepros-plugins
mkdir -p .claude
cp -r /tmp/nimblepros-plugins/plugins/dotnet-testing/commands .claude/
cp -r /tmp/nimblepros-plugins/plugins/dotnet-testing/agents .claude/
cp -r /tmp/nimblepros-plugins/plugins/dotnet-testing/skills .claude/
```

Commit the `.claude/` directory so the whole team gets it.

---

## Usage

When installed as a plugin, commands and skills are namespaced by the **plugin** name:

```bash
/dotnet-testing:add-coverage [optional: namespace or class name]
/workflow:review
/workflow:commit
```

When installed via Option C (standalone copy into `.claude/`), use the short form
(`/add-coverage`, `/review`, `/commit`).

---

## Migrating from the single `nimblepros` plugin

This repository previously published one plugin containing everything, installed as
`nimblepros@nimblepros`. That plugin no longer exists. Claude Code v2.1.193 and later migrate
automatically — the marketplace records the removal in its `renames` map, so the stale entry is
dropped from your settings with a one-line notice. On earlier versions, or to pick your
replacements explicitly:

```bash
claude plugin uninstall nimblepros@nimblepros
claude plugin marketplace update nimblepros
claude plugin install dotnet-testing@nimblepros   # plus any others you want
```

`/nimblepros:add-coverage` is now `/dotnet-testing:add-coverage`. The marketplace name is
unchanged, so the marketplace itself does not need to be re-added. The rationale for the split is
recorded in
[ADR 0001](devdocs/ADRs/0001-2026-08-13-multi-plugin-marketplace-structure.md).

---

## Repository layout

```text
.
├── .claude-plugin/
│   └── marketplace.json          # catalog of the plugins below
├── plugins/
│   ├── dotnet/
│   │   ├── .claude-plugin/plugin.json
│   │   ├── README.md
│   │   ├── skills/
│   │   └── agents/
│   ├── dotnet-testing/
│   │   ├── .claude-plugin/plugin.json
│   │   ├── README.md
│   │   ├── commands/
│   │   ├── agents/
│   │   └── skills/
│   ├── testing/
│   ├── architecture/
│   └── workflow/
├── devdocs/
│   └── ADRs/                     # architecture decision records
├── tests/
│   └── validate_marketplace.py   # structural validation (python tests/validate_marketplace.py)
└── README.md
```

## Contributing

- New content goes in the plugin matching the **capability** it serves — not in a plugin grouped
  by artifact type. Skills, agents, and commands that work together ship together.
- Language-specific guidance stays out of language-agnostic plugins, and vice versa.
- A skill must ship in the same plugin as any agent or command that loads it; no plugin here
  depends on another being installed.
- Keep each `SKILL.md` under ~500 lines, moving detail into supporting files so agents can skip
  loading what they don't need.
- Structural decisions get an ADR in [`devdocs/ADRs/`](devdocs/ADRs/).
