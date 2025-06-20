﻿using System.Collections.Generic;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

public class BoolUserSetting : TypedUserSetting<bool>, IBoolUserSetting
{
  public override bool IsActive
  {
    get
    {
      if (!Value)
      {
        return false;
      }
      return base.IsActive;
    }
  }

  public override void Initialize(
    ITypedUserSettingMetadata<bool> typedMetadata,
    IUserSettingStorage<bool> storage,
    List<IUserSettingBase> activityCriterias)
  {
    base.Initialize(typedMetadata, storage, activityCriterias);
    ValueChanged += (sender, change) => OnIsActiveInvalidated();
  }
}