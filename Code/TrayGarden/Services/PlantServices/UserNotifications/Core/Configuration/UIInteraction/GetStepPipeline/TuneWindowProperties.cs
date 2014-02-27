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
  public class TuneWindowProperties
  {
    #region Constructors and Destructors

    public TuneWindowProperties()
    {
      this.GlobalTitle = "Tray Garden -- User notifications properties";
      this.Header = "User notifications properties";
      this.ShortName = "UserNotificationsProp";
    }

    #endregion

    #region Properties

    protected string GlobalTitle { get; set; }

    protected string Header { get; set; }

    protected string ShortName { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(UNConfigurationStepArgs args)
    {
      WindowWithBackStateConstructInfo constructInfo = args.StateConstructInfo;
      constructInfo.Header = this.Header;
      constructInfo.GlobalTitle = this.GlobalTitle;
      constructInfo.ShortName = this.ShortName;
      constructInfo.ContentVM = this.GetContentVM(args);
    }

    #endregion

    #region Methods

    protected virtual object GetContentVM(UNConfigurationStepArgs args)
    {
      return args.ConfigurationConstructInfo.ResultControlVM;
    }

    #endregion
  }
}