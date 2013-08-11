using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.RuntimeSettings;
using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.TypesHatcher;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration
{
  public static class UserNotificationsConfiguration
  {
    private static ISettingsBox _serviceSettingsBox;

    public static ISettingsBox ServiceSettingsBox
    {
      get
      {
        if (_serviceSettingsBox != null)
          return _serviceSettingsBox;
        _serviceSettingsBox =
          HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("PlantServices")
                                               .GetSubBox("UserNotificationsService");
        return _serviceSettingsBox;
      }
    }

    #region SettingsKeys

    /*public static readonly string DelayBeforeNormalFadingKey = "DelayBeforeNormalFading";
    public static readonly string DisplayPermanentCloseButtonKey = "DisplayPermanentCloseButton";
    public static readonly string DelayBeforeForceFadingKey = "DelayBeforeForceFading";
    public static readonly string NormalFadingDurationKey = "NormalFadingDuration";
    public static readonly string ForceFadingDurationKey = "ForceFadingDuration";
    public static readonly string PermanentCloseDescriptionPatternKey = "PermanentCloseDescriptionPattern";
    public static readonly string NotificationWindowHeightKey = "NotificationWindowHeight";
    public static readonly string NotificationWindowWidthKey = "NotificationWindowWidth";
    public static readonly string NotificationWindowTopIndentKey = "NotificationWindowTopIndent";
    public static readonly string NotificationWindowMaxDisplayedKey = "NotificationWindowHeight";*/

    #endregion

    #region Mediators

    #region Durations

    public static readonly TimeSpanSettingMediator DelayBeforeNormalFading =
      new TimeSpanSettingMediator("DelayBeforeNormalFading", 5000, GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator DelayBeforeForceFading =
      new TimeSpanSettingMediator("DelayBeforeForceFadingKey", 0, GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator NormalFadingDuration =
      new TimeSpanSettingMediator("NormalFadingDuration", 2000, GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator ForceFadingDuration =
      new TimeSpanSettingMediator("ForceFadingDuration", 1000, GetServiceSettingsBox);

    #endregion

    public static readonly BoolSettingMediator DisplayPermanentCloseButton =
      new BoolSettingMediator("DisplayPermanentCloseButton", true, GetServiceSettingsBox);

    public static readonly StringSettingMediator PermanentCloseDescriptionPattern =
      new StringSettingMediator("PermanentCloseDescriptionPattern",
                                "Disable such notifications at all! \r\n#Notification originator: {0}",
                                GetServiceSettingsBox);

    public static readonly IntSettingMediator WorkerTimePeriod = new IntSettingMediator("WorkerTimePeriod", 300,
                                                                                        GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator ExpirationOfInvisibleNotification =
      new TimeSpanSettingMediator("ExpirationOfInvisibleNotification", 3000, GetServiceSettingsBox);

    

    #region Position and Size

    public static readonly IntSettingMediator NotificationWindowHeight =
      new IntSettingMediator("NotificationWindowHeight", 100, GetServiceSettingsBox);

    public static readonly IntSettingMediator NotificationWindowWidth = new IntSettingMediator(
      "NotificationWindowWidth", 350, GetServiceSettingsBox);

    public static readonly IntSettingMediator NotificationWindowTopIndent = new IntSettingMediator(
      "NotificationWindowTopIndent", 50, GetServiceSettingsBox);

    public static readonly IntSettingMediator NotificationWindowMaxDisplayed = new IntSettingMediator(
      "NotificationWindowMaxDisplayed", 3, GetServiceSettingsBox);

    #endregion



    #endregion


    private static ISettingsBox GetServiceSettingsBox()
    {
      return ServiceSettingsBox;
    }

  }
}
