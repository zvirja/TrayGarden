using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Resources;
using TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;
using TrayGarden.Services.Engine.UI.Intergration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.Stuff.ExtentedEntry;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Integration
{
  [UsedImplicitly]
  public class PlantServiceConfigurator
  {
    public string Description { get; set; }

    public PlantServiceConfigurator()
    {
      Description = "Configure service";
    }

    [UsedImplicitly]
    public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
    {
      //Find setting, related to UserConfiguration service
      var entryRelatedToService = args.ConfigConstructInfo.ConfigurationEntries.FirstOrDefault(
        x => ((ConfigurationPlayerServiceAware)x.RealPlayer).InfoSource is UserNotificationsService);
      Assert.IsNotNull(entryRelatedToService, "Entry cannot be unresloved");
      FillPlayerWithConfigAction(entryRelatedToService.RealPlayer);
    }

    protected virtual void FillPlayerWithConfigAction(IConfigurationAwarePlayer realPlayer)
    {
      realPlayer.AdditionalActions.Add(GetConfigurationAction());
    }

    protected virtual ISettingEntryAction GetConfigurationAction()
    {
      var configureIcon = HatcherGuide<IResourcesManager>.Instance.GetIconResource("configureV1", null);
      Assert.IsNotNull(configureIcon, "Resolved image cannot be null");
      var imageSource = ImageHelper.GetBitmapImageFromBitmapThreadSafe(configureIcon.ToBitmap(), ImageFormat.Png);
      return new BasicSettingEntryAction(imageSource, ShowConfigurationWindow, true, null, Description);
    }

    protected virtual void ShowConfigurationWindow(object obj)
    {
      WindowStepState windowStepState = UNConfigurationStepPipeline.Run();
      WindowWithBackVM.GoAheadWithBackIfPossible(windowStepState);
    }
  }
}
