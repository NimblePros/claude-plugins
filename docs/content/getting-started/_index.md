---
title: "Getting Started"
description: "Install the NimblePros marketplace and its plugins"
summary: "Three installation options, command namespacing, and migration from the old single plugin"
date: 2026-08-13
weight: 20
---

Add the marketplace once, then install only the plugins you want.

## Option A — Claude Code plugin install (recommended)

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install <plugin>@nimblepros
```

This installs that plugin's commands, agents, and skills into your global Claude Code
configuration so they are available in every project. Available plugin names: `dotnet`,
`dotnet-testing`, `testing`, `architecture`, `workflow`, `presenting`.

## Option B — Per-project install via `.claude/settings.json`

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

## Option C — Manual copy (standalone, no marketplace)

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

## Command namespacing

When installed as a plugin (Options A and B), commands and skills are namespaced by the plugin
name:

```bash
/dotnet-testing:add-coverage [optional: namespace or class name]
/workflow:review
/workflow:commit
```

When installed via Option C (standalone copy into `.claude/`), use the short form
(`/add-coverage`, `/review`, `/commit`).

## Requirements

Requirements vary per plugin — each plugin page lists its own. In general:

- `git` and `gh` (GitHub CLI, authenticated via `gh auth login`) for
  [workflow]({{< ref "/workflow" >}}) and the
  [`/dotnet-testing:add-coverage`]({{< ref "/dotnet-testing/add-coverage" >}}) pipeline
- the .NET SDK plus test tooling for [dotnet-testing]({{< ref "/dotnet-testing" >}})
- nothing beyond Claude Code itself for the knowledge-only skills

## Migrating from the single `nimblepros` plugin

This repository previously published one plugin containing everything, installed as
`nimblepros@nimblepros`. That plugin no longer exists. Claude Code v2.1.193 and later migrate
automatically — the marketplace records the removal, so the stale entry is dropped from your
settings with a one-line notice. On earlier versions, or to pick your replacements explicitly:

```bash
claude plugin uninstall nimblepros@nimblepros
claude plugin marketplace update nimblepros
claude plugin install dotnet-testing@nimblepros   # plus any others you want
```

`/nimblepros:add-coverage` is now `/dotnet-testing:add-coverage`. The marketplace name is
unchanged, so the marketplace itself does not need to be re-added.
