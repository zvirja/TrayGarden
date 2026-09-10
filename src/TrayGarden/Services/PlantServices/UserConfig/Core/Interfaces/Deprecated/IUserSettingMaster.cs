using System;
using System.Collections.Generic;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

[Obsolete]
public interface IUserSettingMaster : IUserSetting
{
  void Initialize(IUserSettingMetadataBase metadataBase, ISettingsBox containerSettingsBox, List<IBoolUserSetting> activityCriterias);
}