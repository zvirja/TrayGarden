using JetBrains.Annotations;

using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class TuneWindowProperties
{
  public TuneWindowProperties()
  {
    GlobalTitle = "Tray Garden -- User notifications properties";
    Header = "User notifications properties";
    ShortName = "UserNotificationsProp";
  }

  protected string GlobalTitle { get; set; }

  protected string Header { get; set; }

  protected string ShortName { get; set; }

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