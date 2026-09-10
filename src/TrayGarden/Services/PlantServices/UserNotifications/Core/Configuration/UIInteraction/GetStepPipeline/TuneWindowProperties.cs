using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class TuneWindowProperties : IPipelineProcessor<UNConfigurationStepArgs>
{
  private string GlobalTitle { get; set; } = "Tray Garden -- User notifications properties";

  private string Header { get; set; } = "User notifications properties";

  private string ShortName { get; set; } = "UserNotificationsProp";

  [UsedImplicitly]
  public void Process(UNConfigurationStepArgs args)
  {
    WindowWithBackStateConstructInfo constructInfo = args.StateConstructInfo;
    constructInfo.Header = Header;
    constructInfo.GlobalTitle = GlobalTitle;
    constructInfo.ShortName = ShortName;
    constructInfo.ContentVM = GetContentVM(args);
  }

  private object GetContentVM(UNConfigurationStepArgs args)
  {
    return args.ConfigurationConstructInfo.ResultControlVM;
  }
}
