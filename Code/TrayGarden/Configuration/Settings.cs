#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration
{
  public static class Settings
  {
    #region Public Properties

    public static string ApplicationDataFolderName
    {
      get
      {
        return Factory.Instance.GetStringSetting("ApplicationData.FolderName", "TrayGarden");
      }
    }

    #endregion
  }
}