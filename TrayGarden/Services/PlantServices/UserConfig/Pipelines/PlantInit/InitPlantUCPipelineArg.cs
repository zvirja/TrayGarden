using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Reception.Services;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit
{
  public class InitPlantUCPipelineArg : PipelineArgs
  {
    #region Constructors and Destructors

    public InitPlantUCPipelineArg(string luggageName, IPlantEx relatedPlant)
    {
      this.LuggageName = luggageName;
      this.RelatedPlant = relatedPlant;
    }

    #endregion

    #region Public Properties

    public string LuggageName { get; set; }

    public IPersonalUserSettingsSteward PersonalSettingsSteward { get; set; }

    public UserConfigServicePlantBox PlantBox { get; set; }

    public IPlantEx RelatedPlant { get; set; }

    public ISettingsBox SettingBox { get; set; }

    public IUserConfiguration Workhorse { get; set; }

    #endregion
  }
}