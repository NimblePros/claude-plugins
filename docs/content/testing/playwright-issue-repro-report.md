---
title: "playwright-issue-repro-report"
description: "Reproduce GitHub issues with Playwright and publish evidence-based triage comments"
summary: "From narrative bug report to deterministic repro with artifacts"
date: 2026-08-13
weight: 10
---

> Reproduce GitHub issues with Playwright, collect screenshot/video/trace artifacts, and publish
> evidence-based issue comments. Use when a bug report needs reproducible UI steps and supporting
> artifacts for triage.

## What it covers

Turning a narrative bug report into a deterministic, evidence-backed triage artifact:

1. Extract expected vs. observed behaviour and preconditions from the issue.
2. Draft or adapt a Playwright test using semantic locators and user-visible assertions — no hard
   sleeps, no implementation-detail assertions.
3. Run it, collecting `failure.png`, `trace.zip`, and video artifacts.
4. Publish a ready-to-post GitHub comment stating repro status, environment, exact steps, and
   artifact links.

Its guardrails are the point: never claim reproduction unless the assertion *and* the artifacts
agree; always classify the outcome as **reproduced**, **not reproduced**, or **inconclusive**;
never include secrets in issue comments.

## Ships with

| File | Contents |
|---|---|
| `references/WORKFLOW.md` | The seven-step operational reference from gathering issue context through posting the comment and deciding next action |
| `templates/issue-repro-prompt.md` | Fill-in-the-blank prompt for kicking off a repro session |
| `templates/issue-comment-template.md` | The comment skeleton: repro status, environment, steps, observed vs. expected, evidence, notes |

## Cross-references

Requires Playwright and posts comments via the `gh` CLI (or manual upload in the GitHub web UI).
The worked examples in the supporting files are .NET-flavored (`dotnet test`, a C# Playwright
project) even though the workflow itself is stack-neutral — adapt the commands to your stack. It
references no other skills or agents; the `e2e-tester` agent in this plugin is its natural
companion for maintaining the broader e2e suite.
