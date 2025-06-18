using System.Configuration;

namespace TrayGarden.Helpers;

public static class AppConfigHelper
{
  public static bool GetBoolSetting(string name, bool defaultValue)
  {
    string setting = ConfigurationManager.AppSettings[name];
    if (setting.IsNullOrEmpty())
    {
      return defaultValue;
    }
    bool boolValue;
    if (bool.TryParse(setting, out boolValue))
    {
      return boolValue;
    }
    return defaultValue;
  }
}