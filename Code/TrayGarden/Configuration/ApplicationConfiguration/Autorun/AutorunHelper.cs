using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Microsoft.Win32;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun;

public class AutorunHelper : IAutorunHelper
{
  public static readonly string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

  public static readonly string ValueName = "TrayGarden";

  public bool IsAddedToAutorun
  {
    get
    {
      try
      {
        RegistryKey autorunKey = this.GetAppropriateKey();
        var currentValue = autorunKey.GetValue(ValueName) as string;
        if (currentValue.IsNullOrEmpty())
        {
          return false;
        }
        return currentValue.Equals(this.GetExecutablePath(), StringComparison.OrdinalIgnoreCase);
      }
      catch (Exception ex)
      {
        Log.Error("Unable to read startup properties", ex, this);
        return false;
      }
    }
  }

  public bool SetNewAutorunValue(bool runAtStartup)
  {
    try
    {
      RegistryKey autorunKey = this.GetAppropriateKey();
      if (runAtStartup)
      {
        autorunKey.SetValue(ValueName, this.GetExecutablePath(), RegistryValueKind.String);
      }
      else
      {
        autorunKey.DeleteValue(ValueName);
      }
      return true;
    }
    catch (Exception ex)
    {
      Log.Error("Unable to set autorun value", ex, this);
      return false;
    }
  }

  protected virtual RegistryKey GetAppropriateKey()
  {
    return Registry.CurrentUser.OpenSubKey(KeyPath, true);
  }

  protected virtual string GetExecutablePath()
  {
    return Assembly.GetEntryAssembly().Location;
  }
}