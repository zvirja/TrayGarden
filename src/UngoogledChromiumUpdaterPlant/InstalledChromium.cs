using System;
using System.IO;

using Microsoft.Win32;

namespace UngoogledChromiumUpdaterPlant;

/// <summary>
/// The installation as recorded by the Chromium installer in its uninstall registry entry. The entry carries only the
/// four-part Chromium version; the packaging revision of the release tag is not recorded anywhere.
/// </summary>
public sealed record InstalledChromium(Version Version, string InstallLocation)
{
  private const string UninstallKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Uninstall\Chromium";

  public string ExePath => Path.Combine(InstallLocation, "chrome.exe");

  public static InstalledChromium TryDetect()
  {
    return TryRead(RegistryHive.CurrentUser, RegistryView.Default)
           ?? TryRead(RegistryHive.LocalMachine, RegistryView.Registry64)
           ?? TryRead(RegistryHive.LocalMachine, RegistryView.Registry32);
  }

  private static InstalledChromium TryRead(RegistryHive hive, RegistryView view)
  {
    try
    {
      using RegistryKey baseKey = RegistryKey.OpenBaseKey(hive, view);
      using RegistryKey key = baseKey.OpenSubKey(UninstallKeyPath);
      if (key == null)
      {
        return null;
      }

      string versionText = key.GetValue("DisplayVersion") as string;
      string location = key.GetValue("InstallLocation") as string;
      if (!Version.TryParse(versionText, out Version version) || string.IsNullOrWhiteSpace(location))
      {
        return null;
      }

      var installed = new InstalledChromium(version, location);
      return File.Exists(installed.ExePath) ? installed : null;
    }
    catch (Exception)
    {
      return null;
    }
  }
}
