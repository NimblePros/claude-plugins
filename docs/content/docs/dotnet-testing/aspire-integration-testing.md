---
title: "aspire-integration-testing"
description: "Integration tests with .NET Aspire's testing facilities and real dependencies"
summary: "Real databases, real services, dynamic ports, health-checked fixtures"
date: 2026-08-13
weight: 60
---

> Write integration tests using .NET Aspire's testing facilities with xUnit. Covers test
> fixtures, distributed application setup, endpoint discovery, and patterns for testing ASP.NET
> Core apps with real dependencies.

## What it covers

Six core principles: real dependencies over mocks, dynamic port binding, `IAsyncLifetime`
fixture lifecycle, runtime endpoint discovery (never hard-coded URLs), xUnit collections for
parallel isolation, and always waiting on health checks before running tests.

The canonical pattern is an `AspireAppFixture` built with
`DistributedApplicationTestingBuilder`, with explicit startup and health timeouts, shared across
a test collection. The skill also flags a non-obvious gotcha: disabling config-reload-on-change
via a `[ModuleInitializer]`, because each test host's file watcher otherwise exhausts Linux
inotify limits in CI. A troubleshooting table covers timeouts, port conflicts, flaky timing, and
dynamic connection strings.

## Ships with

| File | Contents |
|---|---|
| `advanced-patterns.md` | Endpoint-discovery helpers, database testing with Respawn resets, an Aspire + Playwright combined fixture, conditional resource configuration, test-auth and fake-external-service patterns, message-queue tests |
| `ci-and-tooling.md` | GitHub Actions workflow, resource-readiness retry loops, and Aspire CLI + MCP integration so Claude can query live resource state |

## Cross-references

This is reference material (not slash-invocable, and not part of the
[`/add-coverage`]({{< ref "add-coverage" >}}) pipeline) — Claude loads it when writing
integration tests, i.e. rung 2 of the
[test-type-selection]({{< ref "test-type-selection" >}}) ladder. External dependencies:
`Aspire.Hosting.Testing`, Docker, and optionally Respawn, TestContainers-style resources, and
the Aspire CLI.
