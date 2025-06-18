using System.Collections.Generic;
using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM;
using TrayGarden.UI.Configuration.RuntimeSettingsIntegration;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class AddConfigurationEntries
{
  [UsedImplicitly]
  public virtual void Process(UNConfigurationStepArgs args)
  {
    ConfigurationControlConstructInfo constructInfo = args.ConfigurationConstructInfo;
    this.AddEntries(constructInfo.ConfigurationEntries);
  }

  protected virtual void AddEntries(List<ConfigurationEntryBaseVM> output)
  {
    output.Add(
      new BoolConfigurationEntryVM(
        new PlayerForBoolRSMediator("Display PermanentClose button", UserNotificationsConfiguration.DisplayPermanentCloseButton)));
    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForIntRSMediator("Notification window height (px)", UserNotificationsConfiguration.NotificationWindowHeight)));
    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForIntRSMediator("Notification window width (px)", UserNotificationsConfiguration.NotificationWindowWidth)));
    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForIntRSMediator("Notification window top indent (px)", UserNotificationsConfiguration.NotificationWindowTopIndent)));
    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForIntRSMediator("Max notification window displayed", UserNotificationsConfiguration.NotificationWindowMaxDisplayed)));

    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForTimespanMediator(
          "Expiration of invisible notification (ms)",
          UserNotificationsConfiguration.ExpirationOfInvisibleNotification)));

    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForTimespanMediator("Delay before normal fading (ms)", UserNotificationsConfiguration.DelayBeforeNormalFading)));
    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForTimespanMediator("Delay before force fading (ms)", UserNotificationsConfiguration.DelayBeforeForceFading)));
    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForTimespanMediator("Normal fading duration (ms)", UserNotificationsConfiguration.NormalFadingDuration)));
    output.Add(
      new IntConfigurationEntryVM(
        new PlayerForTimespanMediator("Force fading duration (ms)", UserNotificationsConfiguration.ForceFadingDuration)));
  }
}