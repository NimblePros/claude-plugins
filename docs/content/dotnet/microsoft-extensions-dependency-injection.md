---
title: "microsoft-extensions-dependency-injection"
description: "Organize DI registrations into composable IServiceCollection extension methods"
summary: "Clean Program.cs, reusable registrations in tests"
date: 2026-08-13
weight: 40
---

> Organize DI registrations using IServiceCollection extension methods. Group related services
> into composable Add* methods for clean Program.cs and reusable configuration in tests.

## What it covers

The cure for the 200-line `Program.cs`: group registrations into chainable extension methods
(`AddUserServices()`, `AddEmailServices()`), each returning `IServiceCollection`, placed next to
the feature they register.

- **Naming conventions** — `Add{Feature}Services` / `Configure{Feature}` / `Use{Feature}` (for
  middleware), in files named `{Feature}ServiceCollectionExtensions.cs`.
- **Layered composition** — `AddAppServices()` → `AddDomainServices()` → feature methods.
- **The real payoff: tests** — tests reuse the production `Add*` methods and override only what
  differs, instead of re-registering everything by hand.
- **Lifetime pitfalls** — capturing a scoped service in a singleton, and using scoped services in
  a `BackgroundService` without `IServiceScopeFactory`.

## Ships with

| File | Contents |
|---|---|
| `advanced-patterns.md` | Testing with DI extensions (`WebApplicationFactory`, standalone `ServiceCollection`, Akka.Hosting.TestKit), actor scope management, and conditional/factory/keyed registrations |

## Cross-references

Points to [microsoft-extensions-configuration]({{< ref "microsoft-extensions-configuration" >}})
for the Options pattern, and to an `akka-hosting-actor-patterns` skill (from the separate
NimblePros dotnet-skills collection, not this marketplace) for the Akka-specific patterns its
advanced examples touch on.
