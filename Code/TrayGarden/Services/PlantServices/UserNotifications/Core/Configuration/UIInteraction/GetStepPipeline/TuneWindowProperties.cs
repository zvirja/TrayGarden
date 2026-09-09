using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class TuneWindowProperties : IPipelineProcessor<UNConfigurationStepArgs>
{
  protected string GlobalTitle { get; set; } = "Tray Garden -- User notifications properties";

  protected string Header { get; set; } = "User notifications properties";

  protected string ShortName { get; set; } = "UserNotificationsProp";

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
