using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace SmallApplicationLauncher
{
  public class UserConfiguration : IUserConfiguration
  {
    private static UserConfiguration configuration;

    private UserConfiguration()
    {
      this.Applications = new Dictionary<string, string>();
    }

    public static UserConfiguration Configuration
    {
      get
      {
        return configuration ?? (configuration = new UserConfiguration());
      }
    }

    public Dictionary<string, string> Applications { get; private set; }

    public void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
    {
      IStringUserSetting userSetting = personalSettingsSteward.DeclareStringSetting(
        "Path to Small Apps folder",
        "Path to Small Apps folder",
        @"C:\Apps");
      var folderInfo = new DirectoryInfo(userSetting.Value);
      this.Applications = new Dictionary<string, string>();
      if (folderInfo.Exists)
      {
        foreach (FileInfo app in folderInfo.GetFiles("*.exe"))
        {
          this.Applications.Add(app.Name, app.FullName);
        }
      }
    }
  }
}