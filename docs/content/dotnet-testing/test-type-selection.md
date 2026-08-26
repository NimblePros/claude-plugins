---
title: "test-type-selection"
description: "Unit, integration, or end-to-end: pick the cheapest test that can observe the behaviour"
summary: "The three-rung decision ladder"
date: 2026-08-13
weight: 40
---

> Decide which kind of test to write — unit, integration, or end-to-end (Playwright) — for a
> given behaviour. Prefer the cheapest test type that can actually observe the behaviour. Load
> this skill before choosing where and how to test a coverage gap.

## What it covers

A three-rung ladder with one rule: move down a rung only because the behaviour is genuinely
unobservable at the current one — never for convenience.

1. **Unit** — zero out-of-process dependencies; everything in-process (Blazor component rendering
   via bunit counts as unit).
2. **Integration** — the behaviour *is* the interaction with infrastructure or a realistic
   composition of services: real database via TestContainers, `WebApplicationFactory`, message
   broker.
3. **End-to-end** — the behaviour only exists in a real browser (JS interop, navigation,
   downloads); automated with Playwright, in a dedicated `tests/<App>.E2eTests/` project that is
   never silently created.

The most practical part is the "signals you chose the wrong level" list: mocks that re-implement
collaborator logic mean move down; incidental infrastructure means move up; e2e "for extra
confidence" means move up. Most coverage gaps resolve to unit tests, since CRAP scoring targets
complex in-process logic.

## Cross-references

Defers all naming/structure conventions to [test-authoring]({{< ref "test-authoring" >}}). Loaded
by the `test-author` agent (as its decision procedure) and the `test-reviewer` agent (which
checks the chosen level was right) in the [`/add-coverage`]({{< ref "add-coverage" >}}) pipeline.
For deep integration-test patterns at rung 2, see
[aspire-integration-testing]({{< ref "aspire-integration-testing" >}}). Single-file skill.
