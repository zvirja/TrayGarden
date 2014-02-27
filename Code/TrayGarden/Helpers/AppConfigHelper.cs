#region

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Helpers
{
  public static class AppConfigHelper
  {
    #region Public Methods and Operators

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

    #endregion
  }
}