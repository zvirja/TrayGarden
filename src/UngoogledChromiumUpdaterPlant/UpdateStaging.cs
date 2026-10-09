using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace UngoogledChromiumUpdaterPlant;

public sealed record PendingInstaller(string Path, string Tag, Version ChromiumVersion);

/// <summary>
/// Downloads go to one fixed folder in the temp directory, which is safe because only one TrayGarden instance runs. A file
/// gets its installer name only after size and SHA-256 were verified and it was renamed from '.partial', so a complete
/// installer found there later is trustworthy and an interrupted download is just a '.partial' leftover that gets removed.
/// </summary>
public static class UpdateStaging
{
  private const string PartialExtension = ".partial";

  private static readonly string StagingDirectory = Path.Combine(Path.GetTempPath(), "TrayGarden-UngoogledChromiumUpdater");

  /// <summary>
  /// Looks for an installer left by an earlier run and removes everything else in the folder.
  /// </summary>
  /// <returns>The newest complete installer for this machine, or null.</returns>
  public static PendingInstaller FindPending()
  {
    PendingInstaller best = null;
    if (Directory.Exists(StagingDirectory))
    {
      foreach (string path in EnumerateFiles())
      {
        if (ReleaseClient.TryParseInstallerName(Path.GetFileName(path), out string tag, out Version version)
            && (best == null || version > best.ChromiumVersion))
        {
          best = new PendingInstaller(path, tag, version);
        }
      }
    }

    Sweep(best?.Path);
    return best;
  }

  /// <returns>The path of the verified installer.</returns>
  public static async Task<string> DownloadAsync(ReleaseInfo release, CancellationToken cancellationToken)
  {
    Directory.CreateDirectory(StagingDirectory);
    string finalPath = Path.Combine(StagingDirectory, release.AssetName);
    string partialPath = finalPath + PartialExtension;
    TryDelete(partialPath);
    try
    {
      await DownloadToFileAsync(release, partialPath, cancellationToken);
      if (!await IsValidAsync(partialPath, release, cancellationToken))
      {
        throw new InvalidDataException($"Downloaded file '{release.AssetName}' failed size/SHA-256 verification.");
      }

      File.Move(partialPath, finalPath, overwrite: true);
    }
    catch (Exception)
    {
      TryDelete(partialPath);
      throw;
    }

    Sweep(finalPath);
    return finalPath;
  }

  public static bool IsStillAvailable(string installerPath)
  {
    return installerPath != null && File.Exists(installerPath);
  }

  public static void Delete(string installerPath)
  {
    TryDelete(installerPath);
  }

  /// <summary>
  /// Deletes every file in the folder except <paramref name="keepInstallerPath" />. Locked files are skipped and retried by
  /// the next sweep.
  /// </summary>
  public static void Sweep(string keepInstallerPath)
  {
    if (!Directory.Exists(StagingDirectory))
    {
      return;
    }

    try
    {
      foreach (string path in EnumerateFiles().ToList())
      {
        if (!string.Equals(path, keepInstallerPath, StringComparison.OrdinalIgnoreCase))
        {
          TryDelete(path);
        }
      }
    }
    catch (IOException)
    {
    }
    catch (UnauthorizedAccessException)
    {
    }
  }

  private static IEnumerable<string> EnumerateFiles()
  {
    return Directory.EnumerateFiles(StagingDirectory);
  }

  private static async Task DownloadToFileAsync(ReleaseInfo release, string partialPath, CancellationToken cancellationToken)
  {
    using var response = await ReleaseClient.OpenDownloadAsync(release.DownloadUrl, cancellationToken);
    await using var source = await response.Content.ReadAsStreamAsync(cancellationToken);
    await using var target = new FileStream(partialPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920, useAsync: true);

    await source.CopyToAsync(target, cancellationToken);
  }

  private static async Task<bool> IsValidAsync(string path, ReleaseInfo release, CancellationToken cancellationToken)
  {
    var info = new FileInfo(path);
    if (!info.Exists || (release.Size > 0 && info.Length != release.Size))
    {
      return false;
    }

    if (string.IsNullOrEmpty(release.Sha256))
    {
      return true;
    }

    await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true);
    byte[] hash = await SHA256.HashDataAsync(stream, cancellationToken);
    return string.Equals(Convert.ToHexString(hash), release.Sha256, StringComparison.OrdinalIgnoreCase);
  }

  private static void TryDelete(string path)
  {
    try
    {
      File.Delete(path);
    }
    catch (IOException)
    {
    }
    catch (UnauthorizedAccessException)
    {
    }
  }
}
