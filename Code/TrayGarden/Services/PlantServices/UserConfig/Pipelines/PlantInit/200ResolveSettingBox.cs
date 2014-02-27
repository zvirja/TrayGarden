#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Helpers;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit
{
  [UsedImplicitly]
  public class ResolveSettingBox
  {
    #region Public Properties

    [UsedImplicitly]
    public string SettingBoxName { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantUCPipelineArg args)
    {
      args.SettingBox = args.RelatedPlant.MySettingsBox.GetSubBox(this.GetSettingName());
    }

    #endregion

    #region Methods

    protected virtual string GetSettingName()
    {
      if (this.SettingBoxName.NotNullNotEmpty())
      {
        return this.SettingBoxName;
      }
      return "UserConfigService";
    }

    #endregion
  }
}