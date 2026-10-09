using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace UngoogledChromiumUpdaterPlant;

public sealed record ReleaseInfo(Version ChromiumVersion, string Tag, string AssetName, string DownloadUrl, long Size, string Sha256);

public static class ReleaseClient
{
  private const string SiteRoot = "https://ungoogled-software.github.io/ungoogled-chromium-binaries/";
  private const string FeedUrl = SiteRoot + "feed.xml";
  private const string ConfigRoot = SiteRoot + "config/platforms/windows/";

  private static readonly XNamespace AtomNamespace = "http://www.w3.org/2005/Atom";

  private static readonly HttpClient Http = CreateHttpClient();

  /// <summary>
  /// The release tag looks like '154.0.8037.97-1.1'. Only the part before the hyphen is the Chromium version.
  /// </summary>
  public static bool TryParseChromiumVersion(string tagOrFileFragment, out Version version)
  {
    int hyphen = tagOrFileFragment.IndexOf('-');
    string versionText = hyphen >= 0 ? tagOrFileFragment[..hyphen] : tagOrFileFragment;
    return Version.TryParse(versionText.TrimStart('v'), out version);
  }

  /// <summary>
  /// Parses 'ungoogled-chromium_154.0.8037.97-1.1_installer_x64.exe' for this machine's architecture.
  /// </summary>
  public static bool TryParseInstallerName(string fileName, out string tag, out Version version)
  {
    const string prefix = "ungoogled-chromium_";
    string suffix = $"_installer_{GetInstallerArchitecture()}.exe";
    tag = null;
    version = null;
    if (!fileName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) || !fileName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
    {
      return false;
    }

    tag = fileName[prefix.Length..^suffix.Length];
    return TryParseChromiumVersion(tag, out version);
  }

  private static string GetBinariesArchitecture()
  {
    return RuntimeInformation.OSArchitecture switch
    {
      Architecture.Arm64 => "arm64",
      Architecture.X86 => "32bit",
      _ => "64bit"
    };
  }

  public static string GetInstallerArchitecture()
  {
    return RuntimeInformation.OSArchitecture switch
    {
      Architecture.Arm64 => "arm64",
      Architecture.X86 => "x86",
      _ => "x64"
    };
  }

  /// <summary>
  /// The Atom feed only tells which build is the newest for this architecture. The hash and download URL come from the INI
  /// file the site itself is generated from, which is served next to the pages.
  /// </summary>
  /// <returns>Null when the latest published build has no usable installer for this machine.</returns>
  public static async Task<ReleaseInfo> GetLatestAsync(CancellationToken cancellationToken)
  {
    string latestFolder = await FindLatestFolderAsync(cancellationToken);
    if (latestFolder == null)
    {
      return null;
    }

    string ini = await Http.GetStringAsync($"{ConfigRoot}{GetBinariesArchitecture()}/{latestFolder}.ini", cancellationToken);
    string suffix = $"_installer_{GetInstallerArchitecture()}.exe";
    foreach ((string name, Dictionary<string, string> values) in ParseIniSections(ini))
    {
      if (name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
          && TryParseInstallerName(name, out string tag, out Version version)
          && values.TryGetValue("url", out string url)
          && values.TryGetValue("sha256", out string sha256))
      {
        return new ReleaseInfo(version, tag, name, url, 0, sha256);
      }
    }

    return null;
  }

  private static async Task<string> FindLatestFolderAsync(CancellationToken cancellationToken)
  {
    await using Stream stream = await Http.GetStreamAsync(FeedUrl, cancellationToken);
    XDocument feed = await XDocument.LoadAsync(stream, LoadOptions.None, cancellationToken);

    string architecturePath = $"/releases/windows/{GetBinariesArchitecture()}/";
    string latestFolder = null;
    Version latestVersion = null;
    foreach (XElement link in feed.Descendants(AtomNamespace + "entry").Elements(AtomNamespace + "link"))
    {
      string path = link.Attribute("href")?.Value;
      int index = path?.IndexOf(architecturePath, StringComparison.OrdinalIgnoreCase) ?? -1;
      if (index < 0)
      {
        continue;
      }

      string folder = path[(index + architecturePath.Length)..];
      if (TryParseChromiumVersion(folder, out Version version) && (latestVersion == null || version > latestVersion))
      {
        latestVersion = version;
        latestFolder = folder;
      }
    }

    return latestFolder;
  }

  private static IEnumerable<(string Name, Dictionary<string, string> Values)> ParseIniSections(string ini)
  {
    Dictionary<string, string> current = null;
    string currentName = null;
    foreach (string rawLine in ini.Split('\n'))
    {
      string line = rawLine.Trim();
      if (line.StartsWith('[') && line.EndsWith(']'))
      {
        if (current != null)
        {
          yield return (currentName, current);
        }

        currentName = line[1..^1];
        current = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
      }
      else if (current != null && !line.StartsWith('#') && !line.StartsWith(';'))
      {
        int separator = line.IndexOf('=');
        if (separator > 0)
        {
          current[line[..separator].Trim()] = line[(separator + 1)..].Trim();
        }
      }
    }

    if (current != null)
    {
      yield return (currentName, current);
    }
  }

  public static async Task<HttpResponseMessage> OpenDownloadAsync(string url, CancellationToken cancellationToken)
  {
    HttpResponseMessage response = await Http.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
    response.EnsureSuccessStatusCode();
    return response;
  }

  private static HttpClient CreateHttpClient()
  {
    var client = new HttpClient { Timeout = TimeSpan.FromMinutes(30) };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("TrayGarden-UngoogledChromiumUpdater", "1.0"));
    return client;
  }
}
