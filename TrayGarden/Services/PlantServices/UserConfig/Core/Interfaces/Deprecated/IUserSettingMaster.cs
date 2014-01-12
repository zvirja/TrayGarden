using System;
using System.Collections.Generic;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  [Obsolete]
  public interface IUserSettingMaster : IUserSetting
  {
    void Initialize(IUserSettingMetadataBase metadataBase, ISettingsBox containerSettingsBox, List<IBoolUserSetting> activityCriterias);
  }
}
