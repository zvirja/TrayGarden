using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

public class InitPlantUCPipelineArg : PipelineArgs
{
  public InitPlantUCPipelineArg(string luggageName, IPlantEx relatedPlant)
  {
    LuggageName = luggageName;
    RelatedPlant = relatedPlant;
  }

  public string LuggageName { get; set; }

  public IPersonalUserSettingsSteward PersonalSettingsSteward { get; set; }

  public UserConfigServicePlantBox PlantBox { get; set; }

  public IPlantEx RelatedPlant { get; set; }

  public ISettingsBox SettingBox { get; set; }

  public IUserConfiguration Workhorse { get; set; }
}