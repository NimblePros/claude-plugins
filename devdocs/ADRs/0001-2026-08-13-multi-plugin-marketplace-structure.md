# ADR 0001: Publish Multiple Focused Plugins from a Single Marketplace Repository

## Status

Accepted

## Context

This repository started as a single plugin. Before this decision, the layout was:

```text
claude-plugins/
├── .claude-plugin/
│   ├── marketplace.json      # one plugin entry, "source": "."
│   └── plugin.json           # the plugin itself, rooted at the repo
├── agents/                   # 11 agents, flat
├── commands/                 # 1 command (add-coverage)
└── skills/                   # 19 skills, flat
```

Everything in the repository shipped as one unit named `nimblepros`. Installing it to get
`/add-coverage` also installed `csharp-coding-standards`, `adr-writing`, `commit`, `pr`,
`review`, `opentelemetry-dotnet-instrumentation`, and every other skill and agent in the repo.
That has three concrete costs:

1. **No opt-in granularity.** A team doing front-end work still receives the whole .NET testing
   pipeline. A team that only wants ADR guidance receives NuGet publishing agents.
2. **Namespace noise.** Every command and skill is namespaced by the plugin name, so unrelated
   capabilities collide under one prefix (`/nimblepros:...`), which says nothing about what the
   command does.
3. **Unbounded growth.** The set is already 19 skills and 11 agents, and the backlog (issue
   [#1](https://github.com/NimblePros/claude-plugins/issues/1), Playwright test skills) keeps
   adding to it. A flat single plugin gets worse with every addition, not better.

Anthropic's plugin documentation describes a marketplace as a *catalog of one or more plugins*,
and a plugin as a bundle that can contain skills, agents, commands, hooks, and MCP servers. Their
own examples place multiple plugins beneath a `plugins/` directory in one repository: users add
the marketplace once and then install individual plugins from it. The current single-plugin
layout uses the marketplace mechanism but gets none of its selectivity.

## Decision

Restructure the repository as a marketplace containing several focused plugins:

```text
claude-plugins/
├── .claude-plugin/
│   └── marketplace.json          # catalog only; no root plugin.json
├── plugins/
│   ├── <plugin>/
│   │   ├── .claude-plugin/plugin.json
│   │   ├── README.md
│   │   ├── skills/
│   │   ├── agents/
│   │   └── commands/
│   └── ...
├── devdocs/ADRs/
└── README.md
```

Four rules govern what goes where.

### 1. A marketplace is a catalog, not a plugin

`.claude-plugin/marketplace.json` at the repo root lists every plugin with a `./plugins/<name>`
source. The root `.claude-plugin/plugin.json` is deleted — nothing is installable at the repo
root any more. Users run `claude plugin marketplace add NimblePros/claude-plugins` once, then
`claude plugin install <plugin>@nimblepros` for each plugin they want.

### 2. Language-specific and language-agnostic content stay separate

Practices that outlive any one stack — architecture decision records, PR and review workflow,
end-to-end testing strategy — do not live inside a language plugin. Conversely, `.NET`-specific
guidance (xUnit conventions, Stryker.NET invocation, NuGet packaging, EF Core patterns) does not
leak into a plugin a Python or TypeScript team would install.

This is why `.NET` testing content ships as `dotnet-testing` rather than as `testing`: the
add-coverage pipeline hard-codes `dotnet test`, ReportGenerator, Stryker.NET, xUnit, Shouldly,
and NSubstitute. Naming it `testing` would advertise a language-agnostic capability the content
does not deliver. The `testing` plugin holds only content that genuinely is stack-neutral.

### 3. Skills, agents, and commands group by capability, not by artifact type

A plugin is the whole product for one capability. `dotnet-testing` ships the `add-coverage`
command, the four agents that implement its phases, and the four skills those agents load,
because those nine files are one thing. There is no "agents plugin" or "skills plugin".

A corollary that constrains placement: **a skill ships in the same plugin as the agent or command
that loads it.** `test-author` loads `test-authoring` and `test-type-selection`; all three ship
in `dotnet-testing`. No plugin in this marketplace depends on another plugin being installed.

### 4. Detail lives in supporting files, not in one long SKILL.md

Each `SKILL.md` stays under roughly 500 lines and acts as a router:
[progressive disclosure](https://deviq.com/principles/progressive-disclosure/) lets an agent load
`SKILL.md`, decide the task is irrelevant, and pull in none of the supporting material. The
pattern already in use in `csharp-coding-standards` is the model:

```text
csharp-coding-standards/
├── SKILL.md                            # overview + when to load what
├── value-objects-and-patterns.md
├── composition-and-error-handling.md
├── performance-and-api-design.md
└── anti-patterns-and-reflection.md
```

The measure of a good skill folder is how much context an agent can *avoid* loading.

### First-pass plugin set

| Plugin | Scope |
|---|---|
| `dotnet` | C#/.NET practice: coding standards, package management, configuration and DI patterns, OpenTelemetry instrumentation; the `dotnet-agent` and `publish-nuget` agents |
| `dotnet-testing` | The `/add-coverage` pipeline: CRAP analysis, Stryker.NET mutation testing, xUnit authoring conventions, test-type selection, Aspire integration testing, Playwright .NET refactoring |
| `testing` | Stack-neutral testing: the `e2e-tester` agent and Playwright issue-repro reporting |
| `architecture` | Architecture decisions and technical documentation: `adr-writing`, the `technical-writer` agent |
| `workflow` | The issue → spec → implementation → PR loop over the `gh` CLI: feature specification, implement-feature, commit, conventional-commit, pr, review, and the issue/spec/pr agents |

Only plugins with real content are created. The `frontend`, `databases`, and `devops` plugins
sketched in issue #2 are deferred until there is content to put in them — an empty plugin
directory is not installable and advertises a capability that does not exist.

## Consequences

### Positive

- Users install only what they need; a front-end team never sees NuGet agents.
- Command namespaces describe the capability: `/dotnet-testing:add-coverage`,
  `/workflow:review`.
- New content has an obvious home, and the "does this belong to a language or to a practice?"
  question is answered before the file is written rather than after.
- Each plugin versions independently through its own `plugin.json`.
- Per-plugin `README.md` files keep the root README to a catalog overview instead of a growing
  manual for every capability.

### Negative

- **Breaking change for existing users.** The `nimblepros@nimblepros` plugin no longer exists.
  Anyone who installed it must install the specific plugins they want, and
  `/nimblepros:add-coverage` becomes `/dotnet-testing:add-coverage`. Mitigation: the marketplace
  records `"renames": { "nimblepros": null }`, which lets Claude Code v2.1.193+ drop the stale
  entry from user settings with a notice instead of failing with `plugin-not-found`; the root
  README documents the manual path for older versions. The marketplace name (`nimblepros`) is
  unchanged, so `claude plugin marketplace add` does not have to be redone.
- **Duplication pressure across plugins.** Keeping plugins independently installable means a
  skill needed by two plugins must be duplicated or promoted into a shared plugin that both
  document as a prerequisite. Today no skill is needed by two plugins; when one is, that is the
  decision point.
- **More manifests to maintain.** Five `plugin.json` files plus the marketplace catalog can drift
  out of sync. Mitigation: `tests/validate_marketplace.py` ships with this change and checks that
  every `source` in `marketplace.json` resolves to a directory containing
  `.claude-plugin/plugin.json`, that every plugin directory is catalogued, and that no agent or
  command loads a skill from another plugin. `claude plugin validate .` covers the manifest
  schema itself. Neither runs automatically yet — see Future Considerations.
- **Taxonomy is a judgment call and will be wrong somewhere.** `aspire-integration-testing` could
  defensibly sit in `dotnet` rather than `dotnet-testing`; `technical-writer` could sit in a
  future `docs` plugin. Moving a skill between plugins later is a breaking change for whoever
  installed the losing plugin.

## Alternatives Considered

### Option 1: Keep one plugin at the repo root

Leave `source: "."` and continue adding skills to the flat `skills/` folder.

**Rejected because:** it forces all-or-nothing installation of 19 skills and 11 agents that span
.NET, architecture, GitHub workflow, and browser testing. The problems in Context are already
present at the current size and grow with each addition.

### Option 2: One plugin per skill

Publish every skill as its own plugin for maximum selectivity.

**Rejected because:** it breaks cohesive products apart. `/add-coverage` requires four agents and
four skills to function; shipping eight plugins that must all be installed together makes the
install instructions longer than the feature. It also multiplies manifest maintenance by roughly
thirty.

### Option 3: Split by artifact type (`skills`, `agents`, `commands` plugins)

**Rejected because:** it groups files by what they *are* instead of what they *do*, so no single
plugin is usable on its own — the `commands` plugin's `/add-coverage` would silently fail without
the `agents` plugin. Anthropic's documentation describes plugins as capability bundles that
combine these artifact types, which is the opposite of this split.

### Option 4: Separate repository per plugin

**Rejected because:** it multiplies CI, release, and marketplace-registration overhead, and a
marketplace is explicitly designed to catalog multiple plugins from one repository. Cross-cutting
edits (a convention change touching several plugins) would become multi-repo PR chains.

### Option 5: Group by capability under `plugins/`, split language-specific from agnostic (chosen)

Matches Anthropic's documented marketplace model and their own repository layout, gives users
per-capability opt-in, keeps each plugin independently functional, and leaves a clear place to
add the deferred `frontend`, `databases`, and `devops` plugins.

## Future Considerations

- Add `frontend`, `databases`, and `devops` plugins once there is content for them.
- Issue [#1](https://github.com/NimblePros/claude-plugins/issues/1) (Playwright test skills) lands
  in `testing` if stack-neutral, `dotnet-testing` if it depends on xUnit/.NET tooling.
- Audit each `SKILL.md` against the ~500-line guidance and split the long ones into supporting
  files. No current skill exceeds it, so this is maintenance rather than remediation.
- Wire `tests/validate_marketplace.py` and `claude plugin validate .` into CI. The repository has
  no `.github/workflows/` at all today, so both are manual.
- Genericize content that still assumes one specific repository now that it ships to others:
  `technical-writer` hard-codes a Hugo/Geekdoc site layout in its body, and `publish-nuget`
  hard-codes `Nimble.Endpoints`. Both are flagged in their plugin READMEs rather than rewritten
  here, since this change is about structure.
- Revisit whether `architecture` should split into `architecture` and `docs` if technical-writing
  content grows beyond the `technical-writer` agent.

## Related Links

- Issue [#2 — Create a First Pass at Multiple Plugins in Marketplace](https://github.com/NimblePros/claude-plugins/issues/2)
- Implementation plan: `.claude/plans/2026-08-13-issue-2-multi-plugin-marketplace.md`
- [Progressive Disclosure — DevIQ](https://deviq.com/principles/progressive-disclosure/)
- [Claude Code plugin marketplaces](https://docs.claude.com/en/docs/claude-code/plugin-marketplaces)
