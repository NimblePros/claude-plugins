---
title: "opentelemetry-dotnet-instrumentation"
description: "Tracing and metrics instrumentation that never hurts the application it observes"
summary: "ActivitySource, Meter, and telemetry-as-public-API"
date: 2026-08-13
weight: 50
---

> Provides guidance for implementing OpenTelemetry instrumentation in .NET codebases, covering
> tracing (Activities/Spans), metrics, naming conventions, error handling, performance, and API
> design best practices.

## What it covers

Two load-bearing principles: instrumentation must be **resilient** (an instrumentation exception
must never affect application processing; `Activity` may always be null) and telemetry is
**public API** (emitted span names, tag names, and metrics are subject to the same
breaking-change discipline as your code).

- **Tracing** — use `ActivitySource` (not `DiagnosticSource`), guard with `HasListeners()` as the
  zero-allocation fast path, check `IsAllDataRequested` before computing expensive tags, and keep
  runtime data out of operation names (put specifics in `DisplayName`).
- **Metrics** — group instruments in a per-feature `Meter` class; name recording methods after
  the outcome (`OrderProcessingSucceeded`) rather than the mechanism (`RecordX`); one method per
  outcome instead of boolean flags; treat high-cardinality dimensions as opt-in to avoid
  cardinality explosion.
- **Performance rules** — `Stopwatch.GetTimestamp()`/`GetElapsedTime()` instead of
  `Stopwatch.StartNew()`, the `TagList` struct, no LINQ or string interpolation on hot paths.
- **Testing** — assert span parentage and literal span/tag names so telemetry changes are caught
  like any other breaking change.

## Cross-references

Standalone single-file skill; no supporting files and no references to other skills or agents.
Assumes the OpenTelemetry .NET SDK and links out to the OTel semantic conventions. The
observability concerns here complement the [aspire-integration-testing]({{< ref "/dotnet-testing/aspire-integration-testing" >}})
skill in `dotnet-testing`, which tests apps whose telemetry this skill helps you emit.
