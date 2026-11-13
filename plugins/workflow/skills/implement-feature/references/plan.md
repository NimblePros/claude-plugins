# Step 4 — Persist an implementation plan

## Resolve configuration

Repositories differ in where they keep plans and how they verify a build. Run:

```bash
dotnet "${CLAUDE_PLUGIN_ROOT}/scripts/resolve-config.cs"
```

It reads `<repo-root>/.claude/nimblepros.json` and any `NIMBLEPROS_*` environment
variables, which take precedence. It prints the resolved settings, one per line, with
defaults already applied. If the command cannot run for any reason, use these defaults and
carry on. Never stop to ask where files go.

| Setting | Default |
|---|---|
| `plansDir` | `.claude/plans` |
| `verifyCommand` | `dotnet build && dotnet test && dotnet format --verify-no-changes` |

From here on, **"the plan directory"** and **"the verify command"** mean these values.

## Write the plan

Write a phased, task-based markdown plan to the plan directory **before writing any code**.

- Filename: `YYYY-MM-DD-issue-NNN-short-slug.md` (today's date, the issue number, and a
  short kebab-case summary), matching existing files in that folder.
- Put the issue number and link at the top. After a pivot, also link the parent issue.
- Use numbered phases, each with a checklist of concrete tasks (`- [ ] ...`).
- The plan **must** include:
  - **Automated test requirements**: which behaviors get unit, integration, or e2e
    coverage, and in which test projects.
  - **A final review phase**: a review of the full change set by a **separate agent** with
    fresh context, plus a task to address all significant findings.
  - **A final verification phase**: the application builds, all tests pass, and
    formatting is clean, confirmed by running the verify command.
- Record any assumptions made because acceptance criteria were ambiguous.
- Check off tasks as work proceeds so the plan reflects actual progress.

Next: [implement.md](implement.md).
