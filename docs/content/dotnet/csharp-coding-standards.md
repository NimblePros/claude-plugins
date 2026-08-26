---
title: "csharp-coding-standards"
description: "Modern, high-performance C# with records, pattern matching, value objects, and functional-style composition"
summary: "The NimblePros C# house style"
date: 2026-08-13
weight: 10
---

> Write modern, high-performance C# code using records, pattern matching, value objects,
> async/await, Span&lt;T&gt;/Memory&lt;T&gt;, and best-practice API design patterns. Emphasizes
> functional-style programming with C# 12+ features.

## What it covers

The house C# style, built on a set of core principles:

- **Immutability by default** — `record` types and `init` setters; value objects are
  `readonly record struct`, never classes, and never define implicit conversion operators.
- **Pattern matching over branching** — `switch` expressions preferred to `if`/`else` chains.
- **Async everywhere** — every async method takes a `CancellationToken`; libraries use
  `ConfigureAwait(false)`; no blocking on async code.
- **Composition over inheritance** — interfaces plus composition instead of abstract base
  classes.
- **No reflection-based mapping** — AutoMapper, Mapster, and ExpressMapper are explicitly banned
  in favor of hand-written mapping extension methods, with `UnsafeAccessorAttribute` (.NET 8+) as
  the escape hatch for private access.
- **Errors split two ways** — `Result<T, TError>` for expected business failures; exceptions only
  for unexpected/system errors.
- **Performance-aware APIs** — `Span<T>`, `Memory<T>`, and `ArrayPool<T>` on hot paths; accept
  abstractions, return appropriately specific types.

## Ships with

The `SKILL.md` is a hub; detail lives in four supporting files Claude loads only when relevant:

| File | Contents |
|---|---|
| `value-objects-and-patterns.md` | `readonly record struct` value objects, `TypeConverter` for config binding, full C# 8–12 pattern-matching examples |
| `performance-and-api-design.md` | Zero-allocation `Span<T>`/`Memory<T>` code and method-signature design |
| `composition-and-error-handling.md` | Inheritance-to-composition refactors and the full `Result<T, TError>` implementation |
| `anti-patterns-and-reflection.md` | Why reflection mapping is banned, explicit mapping methods, and eight named anti-patterns |

## Cross-references

Standalone — it references no other skills or agents. The `dotnet-agent` in this plugin follows
the same conventions when implementing features, and the
[test-authoring]({{< ref "/dotnet-testing/test-authoring" >}}) skill in `dotnet-testing`
applies the same spirit to test code.
