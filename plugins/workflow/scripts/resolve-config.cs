#!/usr/bin/env dotnet
// Resolves workflow-plugin configuration for whichever repo Claude Code is
// working in. Run with:
//
//   dotnet ./resolve-config.cs
//
// Precedence, highest first:
//   1. Environment variable  (NIMBLEPROS_PLANS_DIR, NIMBLEPROS_SPECS_DIR,
//                             NIMBLEPROS_VERIFY_COMMAND)
//   2. <repo-root>/.claude/nimblepros.json
//   3. Built-in default
//
// Prints one KEY=value line per setting on stdout and nothing else, so the
// output is trivially readable. Always exits 0: a missing or malformed config
// falls back to defaults (with a note on stderr) so the workflow never stalls.

using System.Text.Json;

var defaults = new Dictionary<string, string>
{
  ["plansDir"] = ".claude/plans",
  ["specsDir"] = ".claude/specs",
  ["verifyCommand"] = "dotnet build && dotnet test && dotnet format --verify-no-changes",
};

static string EnvNameFor(string key) =>
  "NIMBLEPROS_" + string.Concat(key.Select(c => char.IsUpper(c) ? "_" + c : $"{c}")).ToUpperInvariant();

static string FindRepoRoot()
{
  var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
  while (dir is not null)
  {
    if (Directory.Exists(Path.Combine(dir.FullName, ".git"))) return dir.FullName;
    dir = dir.Parent;
  }
  return Directory.GetCurrentDirectory();
}

var root = FindRepoRoot();
var configPath = Path.Combine(root, ".claude", "nimblepros.json");
var fromFile = new Dictionary<string, string>();

if (File.Exists(configPath))
{
  try
  {
    using var doc = JsonDocument.Parse(
      File.ReadAllText(configPath),
      new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });

    foreach (var key in defaults.Keys)
    {
      if (doc.RootElement.ValueKind == JsonValueKind.Object &&
          doc.RootElement.TryGetProperty(key, out var value) &&
          value.ValueKind == JsonValueKind.String &&
          !string.IsNullOrWhiteSpace(value.GetString()))
      {
        fromFile[key] = value.GetString()!.Trim();
      }
    }
  }
  catch (JsonException ex)
  {
    Console.Error.WriteLine($"note: {configPath} is not valid JSON ({ex.Message}); using defaults.");
  }
}

foreach (var (key, fallback) in defaults)
{
  var value = Environment.GetEnvironmentVariable(EnvNameFor(key)) is { Length: > 0 } fromEnv ? fromEnv.Trim()
            : fromFile.TryGetValue(key, out var fileValue) ? fileValue
            : fallback;

  Console.WriteLine($"{key}={value.ReplaceLineEndings(" ")}");
}

return 0;
