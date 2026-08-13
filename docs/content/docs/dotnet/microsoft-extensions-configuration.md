---
title: "microsoft-extensions-configuration"
description: "The Options pattern with strongly-typed settings and validation on startup"
summary: "Fail fast on bad configuration"
date: 2026-08-13
weight: 30
---

> Microsoft.Extensions.Options patterns including IValidateOptions, strongly-typed settings,
> validation on startup, and the Options pattern for clean configuration management.

## What it covers

Configuration as a fail-fast discipline: bind settings to POCOs with a
`public const string SectionName`, then
`.BindConfiguration(...).ValidateDataAnnotations().ValidateOnStart()` — with `.ValidateOnStart()`
called out as the critical, frequently-forgotten piece (without it, validation is deferred to
first access).

- **Data Annotations vs. `IValidateOptions<T>`** — a clear dividing line: annotations for simple
  required/range rules, a validator class for cross-property, conditional, or DI-dependent
  checks.
- **Lifetime selection** — when to use `IOptions<T>`, `IOptionsSnapshot<T>`, or
  `IOptionsMonitor<T>` (including `OnChange` in a `BackgroundService`).
- **Anti-patterns** — injecting raw `IConfiguration`, validating in constructors, omitting
  `ValidateOnStart`, and throwing instead of returning `ValidateOptionsResult.Fail` from a
  validator.

## Ships with

| File | Contents |
|---|---|
| `advanced-patterns.md` | Validators with constructor dependencies, named options, a complete production validator example, and unit-testing configuration validators |

## Cross-references

Referenced by [microsoft-extensions-dependency-injection]({{< ref "microsoft-extensions-dependency-injection" >}})
for the Options pattern — registration of validated options typically lives inside the DI
extension methods that skill prescribes.
