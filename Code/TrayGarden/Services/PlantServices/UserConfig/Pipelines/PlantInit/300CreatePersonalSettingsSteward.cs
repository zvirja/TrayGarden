using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

[UsedImplicitly]
public class CreatePersonalSettingsSteward : IPipelineProcessor<InitPlantUCPipelineArg>
{
  [UsedImplicitly]
  public IUserSettingsBuilder SettingsBuilder { get; set; }

  [UsedImplicitly]
  public virtual void Process(InitPlantUCPipelineArg args)
  {
    IUserSettingsBuilder settingsBuilder = GetSettingBuilder(args.SettingBox);
    args.PersonalSettingsSteward = new PersonalUserSettingsSteward(settingsBuilder);
  }

  protected IUserSettingsBuilder GetSettingBuilder(ISettingsBox settingBox)
  {
    return SettingsBuilder ?? new UserSettingsBuilder(settingBox);
  }
}
