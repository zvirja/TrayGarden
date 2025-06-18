using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit
{
  [UsedImplicitly]
  public class ResolveSettingBox
  {
    [UsedImplicitly]
    public string SettingBoxName { get; set; }

    [UsedImplicitly]
    public virtual void Process(InitPlantUCPipelineArg args)
    {
      args.SettingBox = args.RelatedPlant.MySettingsBox.GetSubBox(this.GetSettingName());
    }

    protected virtual string GetSettingName()
    {
      if (this.SettingBoxName.NotNullNotEmpty())
      {
        return this.SettingBoxName;
      }
      return "UserConfigService";
    }
  }
}