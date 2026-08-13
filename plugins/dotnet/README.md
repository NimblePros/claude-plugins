# dotnet

C# and .NET practice — how NimblePros writes, configures, and ships .NET code.

```bash
claude plugin marketplace add NimblePros/claude-plugins
claude plugin install dotnet@nimblepros
```

## Skills

| Skill | What it covers |
|---|---|
| `csharp-coding-standards` | Modern C#: records, pattern matching, value objects, async/await, `Span<T>`, API design. Detail lives in supporting files loaded on demand. |
| `package-management` | Central Package Management and `dotnet add/remove/list` — never hand-editing package XML. |
| `microsoft-extensions-configuration` | The Options pattern, `IValidateOptions`, strongly-typed settings, validation on startup. |
| `microsoft-extensions-dependency-injection` | Grouping registrations into composable `IServiceCollection` extension methods. |
| `opentelemetry-dotnet-instrumentation` | Tracing and metrics: Activities/Spans, naming conventions, error handling, performance. |

## Agents

| Agent | Purpose |
|---|---|
| `dotnet-agent` | Implements server-side features in C#/.NET/ASP.NET Core/EF Core from an approved specification. |
| `publish-nuget` | Version bump, changelog, metadata validation, pack, and push to NuGet.org. |

## Related plugins

`.NET` testing lives in [`dotnet-testing`](../dotnet-testing/) — coverage analysis, mutation
testing, and xUnit conventions.
