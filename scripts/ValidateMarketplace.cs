#!/usr/bin/env dotnet
// Structural validation for this marketplace repository.
//
// Run from anywhere in the repo (requires the .NET 10 SDK or later — this is a file-based app,
// so there is no project file and nothing to restore):
//
//     dotnet run scripts/ValidateMarketplace.cs
//
// Checks that the marketplace catalog, the plugin manifests, and the skill/agent/command
// frontmatter all agree with each other, and that no plugin depends on a skill that ships in a
// different plugin. Exits non-zero and prints every failure if anything is inconsistent.
//
// This validates repository *structure*. Run `claude plugin validate .` alongside it to validate
// the manifest *schema* against the version of Claude Code you are targeting.

using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.RegularExpressions;

List<string> errors = [];
void Fail(string message) => errors.Add(message);

if (FindRepositoryRoot() is not { } root)
{
    Console.Error.WriteLine("Could not locate the repository root (no .claude-plugin/marketplace.json found).");
    return 1;
}

string Rel(string path) => Path.GetRelativePath(root.FullName, path).Replace('\\', '/');

// 1. The marketplace catalog.
var catalogPath = Path.Combine(root.FullName, ".claude-plugin", "marketplace.json");
if (LoadJson(catalogPath) is not { } catalog)
{
    return Report();
}

if (File.Exists(Path.Combine(root.FullName, ".claude-plugin", "plugin.json")))
{
    Fail(".claude-plugin/plugin.json exists — no plugin is rooted at the repo any more; "
       + "plugins live under plugins/<name>/ (see ADR 0001)");
}

if (Text(catalog, "name") is null)
{
    Fail("marketplace.json: missing top-level `name`");
}

if (!catalog.TryGetProperty("owner", out var owner) || Text(owner, "name") is null)
{
    Fail("marketplace.json: missing top-level `owner` object with a `name`");
}

if (!catalog.TryGetProperty("plugins", out var pluginEntries) || pluginEntries.ValueKind != JsonValueKind.Array)
{
    Fail("marketplace.json: missing top-level `plugins` array");
    return Report();
}

HashSet<string> catalogued = [];
foreach (var entry in pluginEntries.EnumerateArray())
{
    var name = Text(entry, "name");
    var source = Text(entry, "source");
    if (name is null || (source is null && !entry.TryGetProperty("source", out _)))
    {
        Fail($"marketplace.json: plugin entry missing name or source: {entry}");
        continue;
    }

    if (!catalogued.Add(name!))
    {
        Fail($"marketplace.json: duplicate plugin name `{name}`");
    }

    if (source is null)
    {
        continue; // object sources (github, npm, ...) are not local paths to check
    }

    if (!source.StartsWith("./", StringComparison.Ordinal))
    {
        Fail($"marketplace.json: `{name}` source `{source}` must be a relative path starting with `./`");
    }

    var pluginDir = Path.GetFullPath(Path.Combine(root.FullName, source));
    if (!Directory.Exists(pluginDir))
    {
        Fail($"marketplace.json: `{name}` source `{source}` is not a directory");
        continue;
    }

    var manifestPath = Path.Combine(pluginDir, ".claude-plugin", "plugin.json");
    if (!File.Exists(manifestPath))
    {
        Fail($"{source}: missing .claude-plugin/plugin.json");
        continue;
    }

    if (LoadJson(manifestPath) is not { } manifest)
    {
        continue;
    }

    if (Text(manifest, "name") is not { } manifestName || manifestName != name)
    {
        Fail($"{Rel(manifestPath)}: name `{Text(manifest, "name")}` does not match the "
           + $"marketplace entry `{name}`");
    }

    if (Text(manifest, "version") is null)
    {
        Fail($"{Rel(manifestPath)}: missing `version`");
    }

    if (Text(manifest, "description") is null)
    {
        Fail($"{Rel(manifestPath)}: missing `description`");
    }
}

// 2. Every plugin directory is catalogued.
var pluginsDir = Path.Combine(root.FullName, "plugins");
if (!Directory.Exists(pluginsDir))
{
    Fail("plugins/ directory is missing");
    return Report();
}

var pluginDirs = Directory.GetDirectories(pluginsDir).Order(StringComparer.Ordinal).ToArray();
foreach (var orphan in pluginDirs.Select(Path.GetFileName).Where(d => !catalogued.Contains(d!)))
{
    Fail($"plugins/{orphan}: not listed in .claude-plugin/marketplace.json");
}

// 3. Nothing is left at the repo root.
foreach (var stale in (string[])["skills", "agents", "commands"])
{
    if (Directory.Exists(Path.Combine(root.FullName, stale)))
    {
        Fail($"{stale}/ still exists at the repo root; it belongs under plugins/<name>/");
    }
}

// 4. The renames map: every value is null or a plugin that still exists.
if (catalog.TryGetProperty("renames", out var renames))
{
    if (renames.ValueKind != JsonValueKind.Object)
    {
        Fail("marketplace.json: `renames` must be an object");
    }
    else
    {
        foreach (var rename in renames.EnumerateObject())
        {
            if (catalogued.Contains(rename.Name))
            {
                Fail($"marketplace.json: `renames` lists `{rename.Name}`, which is still a live plugin");
            }

            if (rename.Value.ValueKind is not JsonValueKind.Null
                && (rename.Value.GetString() is not { } target || !catalogued.Contains(target)))
            {
                Fail($"marketplace.json: `renames` points `{rename.Name}` at unknown plugin `{rename.Value}`");
            }
        }
    }
}

// 5. Skill, agent, and command frontmatter; and the inventory of each plugin.
var counts = new Dictionary<string, int> { ["skills"] = 0, ["agents"] = 0, ["commands"] = 0 };
Dictionary<string, string> skillOwner = [];
Dictionary<string, List<string>> components = [];

foreach (var pluginDir in pluginDirs)
{
    var pluginName = Path.GetFileName(pluginDir);
    List<string> found = [];

    foreach (var skillDir in Directories(pluginDir, "skills"))
    {
        var skillFile = Path.Combine(skillDir, "SKILL.md");
        if (!File.Exists(skillFile))
        {
            Fail($"{Rel(skillDir)}: skill folder has no SKILL.md");
            continue;
        }

        CheckMarkdown(skillFile, ["name", "description"], expectNameMatch: true);
        var skillName = Path.GetFileName(skillDir);
        if (skillOwner.TryGetValue(skillName, out var otherPlugin))
        {
            Fail($"{Rel(skillDir)}: skill `{skillName}` also exists in plugins/{otherPlugin}");
        }

        skillOwner[skillName] = pluginName;
        found.Add(skillFile);
        counts["skills"]++;
    }

    foreach (var agentFile in Files(pluginDir, "agents"))
    {
        CheckMarkdown(agentFile, ["name", "description"], expectNameMatch: true);
        found.Add(agentFile);
        counts["agents"]++;
    }

    foreach (var commandFile in Files(pluginDir, "commands"))
    {
        CheckMarkdown(commandFile, ["description"], expectNameMatch: false);
        found.Add(commandFile);
        counts["commands"]++;
    }

    components[pluginName] = found;
    if (found.Count == 0)
    {
        Fail($"plugins/{pluginName}: has no skills, agents, or commands — an empty plugin is not installable");
    }
}

// 6. No plugin references a skill that ships in a different plugin (ADR 0001): plugins install
//    independently, so a cross-plugin reference is a dangling dependency.
foreach (var (pluginName, files) in components)
{
    foreach (var path in files)
    {
        var text = File.ReadAllText(path);
        foreach (var (skillName, ownerPlugin) in skillOwner.Where(s => s.Value != pluginName))
        {
            // Match how this repo writes skill references: "`name` skill" / "skill `name`".
            var escaped = Regex.Escape(skillName);
            if (Regex.IsMatch(text, $"(`{escaped}`\\s+skills?\\b|\\bskills?\\s+`{escaped}`)"))
            {
                Fail($"{Rel(path)}: references skill `{skillName}`, which ships in "
                   + $"plugins/{ownerPlugin} — plugins must be independently installable");
            }
        }
    }
}

return Report($"Checked {catalogued.Count} plugins, {counts["skills"]} skills, "
            + $"{counts["agents"]} agents, {counts["commands"]} commands.");

int Report(string summary = "")
{
    if (errors.Count > 0)
    {
        Console.Error.WriteLine($"{Environment.NewLine}{errors.Count} problem(s) found:{Environment.NewLine}");
        foreach (var error in errors)
        {
            Console.Error.WriteLine($"  - {error}");
        }

        return 1;
    }

    if (summary.Length > 0)
    {
        Console.WriteLine(summary);
    }

    Console.WriteLine("All structural checks passed.");
    return 0;
}

JsonElement? LoadJson(string path)
{
    try
    {
        return JsonDocument.Parse(File.ReadAllText(path)).RootElement;
    }
    catch (Exception exception) when (exception is IOException or JsonException)
    {
        Fail($"{Rel(path)}: not readable as JSON ({exception.Message})");
        return null;
    }
}

// Parse the top-level `key: value` pairs of a markdown file's YAML frontmatter.
Dictionary<string, string>? Frontmatter(string path)
{
    var lines = File.ReadAllLines(path);
    if (lines.Length == 0 || lines[0].Trim() != "---")
    {
        Fail($"{Rel(path)}: missing YAML frontmatter (no opening ---)");
        return null;
    }

    Dictionary<string, string> fields = [];
    foreach (var line in lines.Skip(1))
    {
        if (line.Trim() == "---")
        {
            return fields;
        }

        // Nested value or comment; only top-level keys matter here.
        if (line.Length == 0 || line[0] is ' ' or '\t' or '#' || !line.Contains(':'))
        {
            continue;
        }

        var separator = line.IndexOf(':');
        fields[line[..separator].Trim()] = line[(separator + 1)..].Trim().Trim('\'', '"');
    }

    Fail($"{Rel(path)}: missing YAML frontmatter (no closing ---)");
    return null;
}

void CheckMarkdown(string path, string[] required, bool expectNameMatch)
{
    if (Frontmatter(path) is not { } fields)
    {
        return;
    }

    foreach (var key in required)
    {
        if (!fields.TryGetValue(key, out var value) || value.Length == 0)
        {
            Fail($"{Rel(path)}: frontmatter is missing a non-empty `{key}`");
        }
    }

    if (expectNameMatch && fields.TryGetValue("name", out var name) && name.Length > 0)
    {
        var expected = Path.GetFileName(path) == "SKILL.md"
            ? Path.GetFileName(Path.GetDirectoryName(path))
            : Path.GetFileNameWithoutExtension(path);

        if (name != expected)
        {
            Fail($"{Rel(path)}: frontmatter name `{name}` does not match its location (`{expected}`)");
        }
    }
}

static IEnumerable<string> Directories(string pluginDir, string kind) =>
    Directory.Exists(Path.Combine(pluginDir, kind))
        ? Directory.GetDirectories(Path.Combine(pluginDir, kind)).Order(StringComparer.Ordinal)
        : [];

static IEnumerable<string> Files(string pluginDir, string kind) =>
    Directory.Exists(Path.Combine(pluginDir, kind))
        ? Directory.GetFiles(Path.Combine(pluginDir, kind), "*.md").Order(StringComparer.Ordinal)
        : [];

// The script lives in <repo>/scripts/, so its parent is the repository root. Falls back to
// walking up from the working directory when the compiled-in path no longer exists.
static DirectoryInfo? FindRepositoryRoot()
{
    static bool IsRoot(DirectoryInfo dir) =>
        File.Exists(Path.Combine(dir.FullName, ".claude-plugin", "marketplace.json"));

    if (Path.GetDirectoryName(ScriptPath()) is { } scriptDir
        && new DirectoryInfo(scriptDir).Parent is { Exists: true } parent
        && IsRoot(parent))
    {
        return parent;
    }

    for (var dir = new DirectoryInfo(Directory.GetCurrentDirectory()); dir is not null; dir = dir.Parent)
    {
        if (IsRoot(dir))
        {
            return dir;
        }
    }

    return null;
}

static string ScriptPath([CallerFilePath] string path = "") => path;

// A string property's value, or null when the property is absent or is not a string.
static string? Text(JsonElement element, string name) =>
    element.ValueKind == JsonValueKind.Object
    && element.TryGetProperty(name, out var value)
    && value.ValueKind == JsonValueKind.String
        ? value.GetString()
        : null;
