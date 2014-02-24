using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit
{
  [UsedImplicitly]
  public class ProvidePlantWithSteward
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantUCPipelineArg args)
    {
      Assert.IsNotNull(args.PersonalSettingsSteward, "Steward cannot be null");
      args.Workhorse.StoreAndFillPersonalSettingsSteward(args.PersonalSettingsSteward);
    }

    #endregion
  }
}