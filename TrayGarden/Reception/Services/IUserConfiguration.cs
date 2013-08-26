using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows plant to declare the user settings. User may change these settings using UI.
  /// </summary>
  public interface IUserConfiguration
  {
    bool GetUserSettingsMetadata(IUserSettingsMetadataBuilder metadataBuilder);
    void StoreUserSettingsBridge(IUserSettingsBridge userSettingsBridge);
  }
}
