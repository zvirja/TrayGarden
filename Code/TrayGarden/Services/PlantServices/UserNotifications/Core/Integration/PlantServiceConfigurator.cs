using System.Drawing.Imaging;
using System.Linq;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Resources;
using TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;
using TrayGarden.UI.Configuration.EntryVM.Players;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Integration;

[UsedImplicitly]
public class PlantServiceConfigurator(IResourcesManager resourcesManager, IPipelineRunner pipelineRunner)
  : IPipelineProcessor<GetStateForServicesConfigurationPipelineArgs>
{
  public string Description { get; set; } = "Configure service";

  [UsedImplicitly]
  public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    //Find setting, related to UserConfiguration service
    var entryRelatedToService =
      args.ConfigConstructInfo.ConfigurationEntries.FirstOrDefault(
        x => ((ConfigurationPlayerService)x.RealPlayer).InfoSource is UserNotificationsService);
    Assert.IsNotNull(entryRelatedToService, "Entry cannot be unresloved");
    FillPlayerWithConfigAction(entryRelatedToService.RealPlayer);
  }

  protected virtual void FillPlayerWithConfigAction(IConfigurationPlayer realPlayer)
  {
    realPlayer.AdditionalActions.Add(GetConfigurationAction());
  }

  protected virtual IConfigurationEntryAction GetConfigurationAction()
  {
    var configureIcon = resourcesManager.GetIconResource("configureV1", null);
    Assert.IsNotNull(configureIcon, "Resolved image cannot be null");
    var imageSource = ImageHelper.GetBitmapImageFromBitmapThreadSafe(configureIcon.ToBitmap(), ImageFormat.Png);
    return new SimpleConfigurationEntryAction(imageSource, ShowConfigurationWindow, true, null, Description);
  }

  protected virtual void ShowConfigurationWindow(object obj)
  {
    var stepArgs = new UNConfigurationStepArgs();
    pipelineRunner.Run(stepArgs);
    WindowWithBackVM.GoAheadWithBackIfPossible(stepArgs.StateConstructInfo.ResultState);
  }
}
