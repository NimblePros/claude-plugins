---
title: "NimblePros Claude Plugins"
description: "A marketplace of Claude Code plugins for .NET, testing, architecture, and development workflow - built by NimblePros"
lead: "A <strong>Claude Code plugin marketplace</strong> from the NimblePros team — install curated skills, agents, and commands for .NET development, testing, architecture, and your issue-to-PR workflow."
geekdocNav: false
geekdocAlign: center
geekdocAnchor: false
---

Welcome to the NimblePros Claude Plugins documentation. This site explains what each plugin in the
marketplace provides, how the skills, agents, and commands inside each plugin work together, and
how to install only the parts you want.

## What is this marketplace?

[NimblePros/claude-plugins](https://github.com/NimblePros/claude-plugins) is a
[Claude Code plugin marketplace](https://code.claude.com/docs/en/plugins): a single repository you
add once, after which you can install any of its six plugins independently. Each plugin bundles
related **skills** (reference knowledge Claude loads on demand), **agents** (specialized
subagents for focused tasks), and — where it makes sense — **commands** (slash commands that
orchestrate multi-step pipelines).

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install dotnet-testing@nimblepros
```

## The six plugins

| Plugin | What you get |
| --- | --- |
| [**dotnet**]({{< ref "/dotnet" >}}) | C#/.NET practice: coding standards, package management, Options/DI patterns, OpenTelemetry, NuGet publishing |
| [**dotnet-testing**]({{< ref "/dotnet-testing" >}}) | The `/dotnet-testing:add-coverage` pipeline, CRAP analysis, Stryker.NET mutation testing, xUnit conventions, Aspire integration testing |
| [**testing**]({{< ref "/testing" >}}) | Stack-neutral testing: end-to-end test authoring, Playwright issue-repro reports |
| [**architecture**]({{< ref "/architecture" >}}) | ADR writing and technical documentation |
| [**workflow**]({{< ref "/workflow" >}}) | Issue → spec → implementation → review → PR, over the `gh` CLI |
| [**presenting**]({{< ref "/presenting" >}}) | Technical presentations: planning, storytelling, humor, code/diagram visuals, live demos, CFP abstracts, and reveal.js decks |

## How the plugins are organized

Content is grouped by **capability**, not by artifact type — skills, agents, and commands that
work together ship together. Two principles shape the split:

- **Language-specific vs. stack-neutral.** The `dotnet` and `dotnet-testing` plugins assume the
  .NET toolchain; `testing`, `architecture`, and `workflow` do not. That is why testing exists
  twice: [`dotnet-testing`]({{< ref "/dotnet-testing" >}}) carries everything that depends on
  `dotnet test`, xUnit, and Stryker.NET, while [`testing`]({{< ref "/testing" >}}) carries
  Playwright/e2e practice that works on any stack.
- **Self-contained plugins.** A skill ships in the same plugin as any agent or command that loads
  it. No plugin depends on another being installed.

The rationale for this structure is recorded in
[ADR 0001](https://github.com/NimblePros/claude-plugins/blob/main/devdocs/ADRs/0001-2026-08-13-multi-plugin-marketplace-structure.md).

## Sections

- **[Getting Started]({{< ref "/getting-started" >}})** — install options, namespacing, and
  migration from the old single plugin
- **[dotnet]({{< ref "/dotnet" >}})**, **[dotnet-testing]({{< ref "/dotnet-testing" >}})**,
  **[testing]({{< ref "/testing" >}})**, **[architecture]({{< ref "/architecture" >}})**,
  **[workflow]({{< ref "/workflow" >}})**, **[presenting]({{< ref "/presenting" >}})** — one section per plugin, with a summary page per
  skill
