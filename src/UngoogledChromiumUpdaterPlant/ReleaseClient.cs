using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace UngoogledChromiumUpdaterPlant;

public sealed record ReleaseInfo(Version ChromiumVersion, string Tag, string AssetName, string DownloadUrl, long Size, string Sha256);

public static class ReleaseClient
{
  private const string LatestReleaseUrl = "https://api.github.com/repos/ungoogled-software/ungoogled-chromium-windows/releases/latest";

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

  public static string GetInstallerArchitecture()
  {
    return RuntimeInformation.OSArchitecture switch
    {
      Architecture.Arm64 => "arm64",
      Architecture.X86 => "x86",
      _ => "x64"
    };
  }

  /// <returns>Null when the latest release has no usable installer for this machine.</returns>
  public static async Task<ReleaseInfo> GetLatestAsync(CancellationToken cancellationToken)
  {
    using var request = new HttpRequestMessage(HttpMethod.Get, LatestReleaseUrl);
    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github+json"));
    using HttpResponseMessage response = await Http.SendAsync(request, cancellationToken);
    response.EnsureSuccessStatusCode();
    await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
    using JsonDocument document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

    JsonElement root = document.RootElement;
    string tag = root.GetProperty("tag_name").GetString();
    if (!TryParseChromiumVersion(tag, out Version version))
    {
      return null;
    }

    string suffix = $"_installer_{GetInstallerArchitecture()}.exe";
    foreach (JsonElement asset in root.GetProperty("assets").EnumerateArray())
    {
      string name = asset.GetProperty("name").GetString();
      if (!name.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
      {
        continue;
      }

      string digest = asset.TryGetProperty("digest", out JsonElement digestElement) ? digestElement.GetString() : null;
      const string digestPrefix = "sha256:";
      string sha256 = digest != null && digest.StartsWith(digestPrefix, StringComparison.OrdinalIgnoreCase)
        ? digest[digestPrefix.Length..]
        : null;

      return new ReleaseInfo(
        version,
        tag,
        name,
        asset.GetProperty("browser_download_url").GetString(),
        asset.GetProperty("size").GetInt64(),
        sha256);
    }

    return null;
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
