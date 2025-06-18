using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

[UsedImplicitly]
public class CreatePersonalSettingsSteward
{
  [UsedImplicitly]
  public IUserSettingsBuilder SettingsBuilder { get; set; }

  [UsedImplicitly]
  public virtual void Process(InitPlantUCPipelineArg args)
  {
    IUserSettingsBuilder settingsBuilder = this.GetSettingBuilder(args.SettingBox);
    args.PersonalSettingsSteward = new PersonalUserSettingsSteward(settingsBuilder);
  }

  protected IUserSettingsBuilder GetSettingBuilder(ISettingsBox settingBox)
  {
    return this.SettingsBuilder ?? new UserSettingsBuilder(settingBox);
  }
}