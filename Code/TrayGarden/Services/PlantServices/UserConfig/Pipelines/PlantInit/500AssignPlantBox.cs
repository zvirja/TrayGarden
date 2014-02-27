#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Services.PlantServices.UserConfig.Core;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit
{
  [UsedImplicitly]
  public class AssignPlantBox
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantUCPipelineArg args)
    {
      UserConfigServicePlantBox userConfigServicePlantBox = this.CreatePlantBox(args);
      args.RelatedPlant.PutLuggage(args.LuggageName, userConfigServicePlantBox);
    }

    #endregion

    #region Methods

    protected virtual UserConfigServicePlantBox CreatePlantBox(InitPlantUCPipelineArg args)
    {
      var plantBox = new UserConfigServicePlantBox();
      plantBox.RelatedPlantEx = args.RelatedPlant;
      plantBox.SettingsBox = args.SettingBox;
      plantBox.SettingsSteward = args.PersonalSettingsSteward;
      return plantBox;
    }

    #endregion
  }
}