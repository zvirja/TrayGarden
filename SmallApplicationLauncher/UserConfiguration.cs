using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmallApplicationLauncher
{
  public class UserConfiguration : IUserConfiguration
  {
    private static UserConfiguration configuration;
    public Dictionary<string, string> Applications { get; private set; }

    public static UserConfiguration Configuration
    {
      get
      {
        return configuration ?? (configuration = new UserConfiguration());
      }
    }

    private UserConfiguration()
    {
      Applications = new Dictionary<string, string>();
    }

    public bool GetUserSettingsMetadata(IUserSettingsMetadataBuilder metadataBuilder)
    {
      metadataBuilder.DeclareStringSetting("Path to Small Apps folder", @"C:\Apps");
      return true;
    }

    public void StoreUserSettingsBridge(IUserSettingsBridge userSettingsBridge)
    {
      var folderInfo = new DirectoryInfo(userSettingsBridge.GetUserSetting("Path to Small Apps folder").StringValue);
      Applications = new Dictionary<string, string>();
      
      foreach (FileInfo app in folderInfo.GetFiles("*.exe"))
      {
        Applications.Add(app.Name, app.FullName);
      }
    }
  }
}