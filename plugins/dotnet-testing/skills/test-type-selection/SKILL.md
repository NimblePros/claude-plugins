---
name: test-type-selection
description: Decide which kind of test to write — unit, integration, or end-to-end (Playwright) — for a given behaviour. Prefer the cheapest test type that can actually observe the behaviour. Load this skill before choosing where and how to test a coverage gap.
invocable: true
---

# Test Type Selection

## The heuristic

**If you can test it with a unit test, do that. If not, if you can test it with an integration test, do that. If not, test it with an end-to-end test.**

Work down the ladder and stop at the first level that can genuinely observe the behaviour you need to verify. Never move down a level for convenience — only because the behaviour is unobservable at the current level.

| Prefer | Type | Speed | Determinism | Cost to maintain |
|---|---|---|---|---|
| 1st | Unit | milliseconds | high | low |
| 2nd | Integration | seconds | medium | medium |
| 3rd | End-to-end | tens of seconds | lowest | high |

---

## Definitions

### Unit test

**No dependency on out-of-process logic.** The test runs entirely in-process: the subject and its collaborators are real in-memory types or test doubles (NSubstitute). No database, no HTTP, no file system, no browser, no clock drift. Blazor component tests via bunit count as unit tests — bunit renders in-process without a browser.

A behaviour is unit-testable when its observable outcome (return value, state change, raised event, call to a mocked collaborator, rendered markup via bunit) can be asserted without crossing a process boundary.

### Integration test

**Leverages infrastructure / out-of-process logic and/or many collaborating types or services.** The test exercises the subject together with real infrastructure or a realistic composition of services: a real database (TestContainers), the ASP.NET Core pipeline (`Microsoft.AspNetCore.Mvc.Testing` / `WebApplicationFactory`), the file system, a message broker, or several services wired through real DI.

Reach for this when the behaviour under test *is* the interaction — a query that only fails against a real database, middleware ordering, serialization over the wire, endpoint routing and status codes.

### End-to-end test

**Tests through the UI by automating user interactions.** Playwright drives a real browser against the running application: navigation, clicks, form input, keyboard gestures, and assertions on what the user actually sees.

Reach for this only when the behaviour lives in the browser itself and cannot be observed in-process: JavaScript interop, real rendering/layout behaviour, browser navigation and history, file downloads/uploads, or a workflow whose correctness spans client and server through the real UI.

---

## Decision procedure

Given a behaviour to test (e.g. a coverage gap), ask in order:

1. **Can the outcome be asserted in-process?** Pure logic, branching, validation, mapping, state machines — and Blazor component rendering/parameters/events via bunit. → **Unit test.** This should be the answer for the large majority of coverage gaps, since CRAP scoring targets complex in-process logic.
2. **Does the behaviour only exist at a process or infrastructure boundary?** Real SQL semantics, EF Core query translation, HTTP pipeline behaviour, auth handshakes, serialization contracts. → **Integration test.**
3. **Does the behaviour only exist in a real browser driven by a user?** JS interop, actual browser rendering, multi-page user workflows. → **End-to-end test with Playwright.**

Signals you chose the wrong level:

- A unit test whose mocks re-implement the collaborator's logic → the behaviour is the interaction; move **down** to integration.
- An integration test where infrastructure is incidental (the DB is only there to return a fixed row) → extract the logic and move **up** to a unit test.
- An e2e test asserting something a bunit or `WebApplicationFactory` test could see → move **up**; keep e2e suites small.
- Wanting e2e "for confidence" rather than because the behaviour is browser-only → move up.

---

## Stack per type

All three types are xUnit test classes and follow the `test-authoring` skill's naming and structure conventions.

| Type | Key libraries |
|---|---|
| Unit | xUnit, Shouldly, NSubstitute, bunit (Blazor components) |
| Integration | + `Microsoft.AspNetCore.Mvc.Testing`, TestContainers (real infrastructure) |
| End-to-end | + `Microsoft.Playwright` (browser automation) |

E2e tests belong in a dedicated test project (conventionally `tests/<App>.E2eTests/` or similar) so their slower, browser-dependent runs don't drag down the unit/integration suites. If the solution has no Playwright test project and no `Microsoft.Playwright` reference, do not silently create one — report the situation and ask the user whether to set one up before writing an e2e test.
