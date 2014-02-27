#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.RuntimeSettings;

#endregion

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class ResolveSettingsBox
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantSIArgs args)
    {
      ISettingsBox settingsBox = args.SIBox.RelatedPlantEx.MySettingsBox.GetSubBox("StandaloneIconService");
      args.SIBox.SettingsBox = settingsBox;
    }

    #endregion
  }
}