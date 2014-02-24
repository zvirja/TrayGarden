using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline
{
  [UsedImplicitly]
  public class TuneWindowProperties
  {
    protected string ShortName { get; set; }
    protected string GlobalTitle { get; set; }
    protected string Header { get; set; }

    public TuneWindowProperties()
    {
      GlobalTitle = "Tray Garden -- User notifications properties";
      Header = "User notifications properties";
      ShortName = "UserNotificationsProp";
    }

    [UsedImplicitly]
    public virtual void Process(UNConfigurationStepArgs args)
    {
      WindowWithBackStateConstructInfo constructInfo = args.StateConstructInfo;
      constructInfo.Header = Header;
      constructInfo.GlobalTitle = GlobalTitle;
      constructInfo.ShortName = ShortName;
      constructInfo.ContentVM = GetContentVM(args);
    }

    protected virtual object GetContentVM(UNConfigurationStepArgs args)
    {
      return args.ConfigurationConstructInfo.ResultControlVM;
    }
  }
}
