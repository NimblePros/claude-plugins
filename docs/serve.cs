#!/usr/bin/env dotnet
#:property PublishAot=false
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;

const string HugoVersion = "0.161.1";
const string GeekdocVersion = "v4.1.0";
const string ThemeDir = "themes/hugo-geekdoc";
const string TarballUrl = $"https://github.com/thegeeklab/hugo-geekdoc/releases/download/{GeekdocVersion}/hugo-geekdoc.tar.gz";

// Auto-download Geekdoc theme if not already present
if (!Directory.Exists(ThemeDir))
{
  Console.WriteLine($"Downloading Geekdoc theme {GeekdocVersion}...");
  Directory.CreateDirectory(ThemeDir);

  var tarball = "themes/hugo-geekdoc.tar.gz";
  using var http = new HttpClient();
  var bytes = await http.GetByteArrayAsync(TarballUrl);
  await File.WriteAllBytesAsync(tarball, bytes);

  RunCommand("tar", $"-xzf {tarball} -C {ThemeDir} --strip-components=1");
  File.Delete(tarball);
  Console.WriteLine("Theme downloaded.");
}

// Ensure the correct Hugo version is available
var hugoExecutable = await EnsureHugoAsync();

Console.WriteLine("Starting NimblePros Claude Plugins Hugo site...");
Console.WriteLine("Site will be available at http://localhost:1316");

// Start Hugo server (run from the docs directory)
return RunCommand(hugoExecutable, "server --disableFastRender --noHTTPCache --port 1316");

async Task<string> EnsureHugoAsync()
{
  // Check if a local pinned copy already exists
  var localBin = GetLocalHugoPath();
  if (File.Exists(localBin))
    return localBin;

  // Check if the system hugo is the expected version
  try
  {
    var versionProc = new Process
    {
      StartInfo = new ProcessStartInfo
      {
        FileName = "hugo",
        Arguments = "version",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
      }
    };
    versionProc.Start();
    var output = await versionProc.StandardOutput.ReadToEndAsync();
    await versionProc.WaitForExitAsync();
    if (versionProc.ExitCode == 0 && output.Contains(HugoVersion))
      return "hugo";
  }
  catch { /* hugo not in PATH */ }

  // Download Hugo extended for this platform
  Console.WriteLine($"Downloading Hugo extended v{HugoVersion}...");
  var downloadUrl = GetHugoDownloadUrl();
  var binDir = Path.GetDirectoryName(localBin)!;
  Directory.CreateDirectory(binDir);
  var tempArchive = Path.Combine(binDir, "hugo-download" + Path.GetExtension(downloadUrl));

  using var http = new HttpClient();
  var bytes = await http.GetByteArrayAsync(downloadUrl);
  await File.WriteAllBytesAsync(tempArchive, bytes);

  if (downloadUrl.EndsWith(".zip"))
  {
    ZipFile.ExtractToDirectory(tempArchive, binDir, overwriteFiles: true);
  }
  else
  {
    RunCommand("tar", $"-xzf {tempArchive} -C {binDir}");
  }

  File.Delete(tempArchive);

  if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
  {
    File.SetUnixFileMode(localBin,
      UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
      UnixFileMode.GroupRead | UnixFileMode.GroupExecute |
      UnixFileMode.OtherRead | UnixFileMode.OtherExecute);
  }

  Console.WriteLine("Hugo downloaded.");
  return localBin;
}

string GetLocalHugoPath()
{
  var ext = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : "";
  return Path.Combine(".hugo-bin", $"hugo{ext}");
}

string GetHugoDownloadUrl()
{
  var os = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "windows"
    : RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "darwin"
    : "linux";
  var arch = RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "arm64" : "amd64";
  var ext = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "zip" : "tar.gz";
  return $"https://github.com/gohugoio/hugo/releases/download/v{HugoVersion}/hugo_extended_{HugoVersion}_{os}-{arch}.{ext}";
}

int RunCommand(string command, string arguments)
{
  var process = new Process
  {
    StartInfo = new ProcessStartInfo
    {
      FileName = command,
      Arguments = arguments,
      UseShellExecute = false
    }
  };

  process.Start();
  process.WaitForExit();
  return process.ExitCode;
}
