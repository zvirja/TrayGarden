using JetBrains.Annotations;

using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

[UsedImplicitly]
public class ResolveSettingBox : IPipelineProcessor<InitPlantUCPipelineArg>
{
  [UsedImplicitly]
  public string SettingBoxName { get; set; }

  [UsedImplicitly]
  public virtual void Process(InitPlantUCPipelineArg args)
  {
    args.SettingBox = args.RelatedPlant.MySettingsBox.GetSubBox(GetSettingName());
  }

  protected virtual string GetSettingName()
  {
    if (SettingBoxName.NotNullNotEmpty())
    {
      return SettingBoxName;
    }
    return "UserConfigService";
  }
}
