#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.ForSimplerLife;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline
{
  [UsedImplicitly]
  public class TuneConfigurationProperties
  {
    #region Constructors and Destructors

    public TuneConfigurationProperties()
    {
      this.ConfigurationDescription = "This window allows to tune the User Nofications service properties";
    }

    #endregion

    #region Public Properties

    public string ConfigurationDescription { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(UNConfigurationStepArgs args)
    {
      ConfigurationControlConstructInfo constructInfo = args.ConfigurationConstructInfo;
      constructInfo.AllowReboot = false;
      constructInfo.EnableResetAllOption = true;
      constructInfo.ConfigurationDescription = this.ConfigurationDescription;
    }

    #endregion
  }
}