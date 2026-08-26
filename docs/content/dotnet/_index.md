---
title: "dotnet"
description: "C# and .NET practice: coding standards, package management, configuration, dependency injection, OpenTelemetry, and NuGet publishing"
summary: "How NimblePros writes, configures, and ships .NET code"
date: 2026-08-13
weight: 30
---

C# and .NET practice — how NimblePros writes, configures, and ships .NET code.

```bash
claude plugin install dotnet@nimblepros
```

This plugin is pure knowledge and agents: five skills and two agents, no slash commands. The
skills are reference material Claude loads on demand while writing .NET code; you don't invoke
them directly.

## Skills

| Skill | One-line summary |
|---|---|
| [csharp-coding-standards]({{< ref "csharp-coding-standards" >}}) | Modern C# 12+: records, pattern matching, value objects, `Span<T>`, and a hard ban on reflection-based mappers |
| [package-management]({{< ref "package-management" >}}) | Central Package Management driven entirely through the `dotnet` CLI — never hand-edit package XML |
| [microsoft-extensions-configuration]({{< ref "microsoft-extensions-configuration" >}}) | The Options pattern with validation on startup |
| [microsoft-extensions-dependency-injection]({{< ref "microsoft-extensions-dependency-injection" >}}) | Composable `IServiceCollection` extension methods instead of a 200-line `Program.cs` |
| [opentelemetry-dotnet-instrumentation]({{< ref "opentelemetry-dotnet-instrumentation" >}}) | Resilient, low-overhead tracing and metrics with `ActivitySource` and `Meter` |

The configuration and dependency-injection skills cross-reference each other — DI extension
methods are where options registration lives, and the DI skill points at the configuration skill
for the Options pattern.

## Agents

**`dotnet-agent`** implements server-side features in C#/.NET/ASP.NET Core/EF Core from an
approved specification: it reads the spec from `specs/` before writing code, works test-first
with xUnit, and updates the spec as it completes work. It pairs naturally with the
[workflow]({{< ref "/workflow" >}}) plugin's spec-producing agents, though neither plugin
requires the other.

**`publish-nuget`** runs a gated NuGet release pipeline: pre-publish checklist (SemVer, release
notes, metadata, zero-warning build, passing tests), then version bump → `RELEASE_NOTES.md`
entry → pack → inspect → push → git tag. It reads the API key from the `NUGET_API_KEY`
environment variable and never publishes from a dirty working tree. Note: its example paths
target the `Nimble.Endpoints` package — point it at your own `.csproj` when using it elsewhere.

## Requirements

The skills need nothing beyond Claude Code. The agents use the `dotnet` CLI, and `publish-nuget`
additionally needs `git` and a `NUGET_API_KEY` environment variable.

## Related plugins

.NET *testing* deliberately lives in [dotnet-testing]({{< ref "/dotnet-testing" >}}) —
coverage analysis, mutation testing, and xUnit conventions.
