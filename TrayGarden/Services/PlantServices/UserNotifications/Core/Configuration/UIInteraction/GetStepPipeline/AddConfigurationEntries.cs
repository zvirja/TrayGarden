using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.UI.Configuration.RuntimeSettingsIntegration;
using TrayGarden.UI.Configuration.Stuff;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline
{
  [UsedImplicitly]
  public class AddConfigurationEntries
  {
    [UsedImplicitly]
    public virtual void Process(UNConfigurationStepArgs args)
    {
      ConfigurationControlConstructInfo constructInfo = args.ConfigurationConstructInfo;
      AddEntries(constructInfo.ConfigurationEntries);
    }

    protected virtual void AddEntries(List<ConfigurationEntryVMBase> output)
    {
      output.Add(
        new ConfigurationEntryForBoolVM(new BoolRSMediatorBasedPlayer("Display PermanentClose button",
                                                                      UserNotificationsConfiguration
                                                                        .DisplayPermanentCloseButton)));
      output.Add(
        new ConfigurationEntryForIntVM(new IntRSMediatorBasedPlayer("Notification window height (px)",
                                                                    UserNotificationsConfiguration
                                                                      .NotificationWindowHeight)));
      output.Add(
        new ConfigurationEntryForIntVM(new IntRSMediatorBasedPlayer("Notification window width (px)",
                                                                    UserNotificationsConfiguration
                                                                      .NotificationWindowWidth)));
      output.Add(
        new ConfigurationEntryForIntVM(new IntRSMediatorBasedPlayer("Notification window top indent (px)",
                                                                    UserNotificationsConfiguration
                                                                      .NotificationWindowTopIndent)));
      output.Add(
        new ConfigurationEntryForIntVM(new IntRSMediatorBasedPlayer("Max notification window displayed",
                                                                    UserNotificationsConfiguration
                                                                      .NotificationWindowMaxDisplayed)));

      output.Add(
        new ConfigurationEntryForIntVM(new TimeSpanMediatorBasedPlayer("Expiration of invisible notification (ms)",
                                                                       UserNotificationsConfiguration
                                                                         .ExpirationOfInvisibleNotification)));

      output.Add(
        new ConfigurationEntryForIntVM(new TimeSpanMediatorBasedPlayer("Delay before normal fading (ms)",
                                                                       UserNotificationsConfiguration
                                                                         .DelayBeforeNormalFading)));
      output.Add(
        new ConfigurationEntryForIntVM(new TimeSpanMediatorBasedPlayer("Delay before force fading (ms)",
                                                                       UserNotificationsConfiguration
                                                                         .DelayBeforeForceFading)));
      output.Add(
        new ConfigurationEntryForIntVM(new TimeSpanMediatorBasedPlayer("Normal fading duration (ms)",
                                                                       UserNotificationsConfiguration
                                                                         .NormalFadingDuration)));
      output.Add(
        new ConfigurationEntryForIntVM(new TimeSpanMediatorBasedPlayer("Force fading duration (ms)",
                                                                       UserNotificationsConfiguration
                                                                         .ForceFadingDuration)));
    }
  }
}
