using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

[UsedImplicitly]
public class ProvidePlantWithSteward : IPipelineProcessor<InitPlantUCPipelineArg>
{
  [UsedImplicitly]
  public void Process(InitPlantUCPipelineArg args)
  {
    Assert.IsNotNull(args.PersonalSettingsSteward, "Steward cannot be null");
    args.Workhorse.StoreAndFillPersonalSettingsSteward(args.PersonalSettingsSteward);
  }
}
