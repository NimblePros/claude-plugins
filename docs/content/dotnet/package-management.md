---
title: "package-management"
description: "Central Package Management and dotnet CLI discipline for NuGet"
summary: "Never hand-edit package XML"
date: 2026-08-13
weight: 20
---

> Manage NuGet packages using Central Package Management (CPM) and dotnet CLI commands. Never
> edit XML directly - use dotnet add/remove/list commands. Use shared version variables for
> related packages.

## What it covers

The golden rule: package changes go through the `dotnet` CLI (`dotnet add/remove/list package`),
never through hand-edited `.csproj` or `Directory.Packages.props` — the CLI validates the package
exists, resolves versions, and handles transitive dependencies.

On top of that:

- **Central Package Management setup** — `ManagePackageVersionsCentrally` with versionless
  `PackageReference` entries.
- **Shared version variables** — MSBuild properties like `$(AspireVersion)` or
  `$(OpenTelemetryVersion)` keep families of related packages in lockstep.
- **When *not* to use CPM** — legacy solutions, version ranges (unsupported), older tooling
  (needs SDK 6.0.300+, NuGet 6.2+, VS 2022 17.2+).
- Troubleshooting (cache clearing, transitive trees, lock files), private feed / `NuGet.config`
  setup, and a quick-reference command table.

## Cross-references

The skill flags `VersionOverride` as something the NimblePros *Slopwatch* tooling detects as
potential slop — that check lives outside this marketplace. Optional companion tooling:
`dotnet-outdated-tool` for bulk upgrades. Single-file skill; no supporting files.
