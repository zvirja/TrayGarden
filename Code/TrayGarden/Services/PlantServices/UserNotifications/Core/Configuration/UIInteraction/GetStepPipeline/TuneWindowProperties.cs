using JetBrains.Annotations;

using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class TuneWindowProperties
{
  public TuneWindowProperties()
  {
    this.GlobalTitle = "Tray Garden -- User notifications properties";
    this.Header = "User notifications properties";
    this.ShortName = "UserNotificationsProp";
  }

  protected string GlobalTitle { get; set; }

  protected string Header { get; set; }

  protected string ShortName { get; set; }

  [UsedImplicitly]
  public virtual void Process(UNConfigurationStepArgs args)
  {
    WindowWithBackStateConstructInfo constructInfo = args.StateConstructInfo;
    constructInfo.Header = this.Header;
    constructInfo.GlobalTitle = this.GlobalTitle;
    constructInfo.ShortName = this.ShortName;
    constructInfo.ContentVM = this.GetContentVM(args);
  }

  protected virtual object GetContentVM(UNConfigurationStepArgs args)
  {
    return args.ConfigurationConstructInfo.ResultControlVM;
  }
}