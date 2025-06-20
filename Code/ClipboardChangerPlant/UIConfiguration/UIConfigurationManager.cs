﻿using System.Collections.Generic;
using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace ClipboardChangerPlant.UIConfiguration;

public class UIConfigurationManager : IUserConfiguration
{
  static UIConfigurationManager()
  {
    ActualManager = new UIConfigurationManager();
  }

  public static UIConfigurationManager ActualManager { get; protected set; }

  public IPersonalUserSettingsSteward SettingsSteward { get; set; }

  public Dictionary<string, IUserSettingBase> UserSettings
  {
    get
    {
      return SettingsSteward.DefinedSettings;
    }
  }

  public virtual void StoreAndFillPersonalSettingsSteward(IPersonalUserSettingsSteward personalSettingsSteward)
  {
    SettingsSteward = personalSettingsSteward;
  }
}