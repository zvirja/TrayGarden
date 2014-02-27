#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.RuntimeSettings;
using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration
{
  public static class UserNotificationsConfiguration
  {
    #region Static Fields

    public static readonly TimeSpanSettingMediator DelayBeforeForceFading = new TimeSpanSettingMediator(
      "DelayBeforeForceFading",
      3000,
      GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator DelayBeforeNormalFading = new TimeSpanSettingMediator(
      "DelayBeforeNormalFading",
      5000,
      GetServiceSettingsBox);

    public static readonly BoolSettingMediator DisplayPermanentCloseButton = new BoolSettingMediator(
      "DisplayPermanentCloseButton",
      true,
      GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator ExpirationOfInvisibleNotification =
      new TimeSpanSettingMediator("ExpirationOfInvisibleNotification", 3000, GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator ForceFadingDuration = new TimeSpanSettingMediator(
      "ForceFadingDuration",
      500,
      GetServiceSettingsBox);

    public static readonly TimeSpanSettingMediator NormalFadingDuration = new TimeSpanSettingMediator(
      "NormalFadingDuration",
      500,
      GetServiceSettingsBox);

    public static readonly IntSettingMediator NotificationWindowHeight = new IntSettingMediator(
      "NotificationWindowHeight",
      110,
      GetServiceSettingsBox);

    public static readonly IntSettingMediator NotificationWindowMaxDisplayed = new IntSettingMediator(
      "NotificationWindowMaxDisplayed",
      3,
      GetServiceSettingsBox);

    public static readonly IntSettingMediator NotificationWindowTopIndent = new IntSettingMediator(
      "NotificationWindowTopIndent",
      50,
      GetServiceSettingsBox);

    public static readonly IntSettingMediator NotificationWindowWidth = new IntSettingMediator(
      "NotificationWindowWidth",
      350,
      GetServiceSettingsBox);

    public static readonly StringSettingMediator PermanentCloseDescriptionPattern =
      new StringSettingMediator(
        "PermanentCloseDescriptionPattern",
        "Disable such notifications at all! \r\n#Notification originator: {0}",
        GetServiceSettingsBox);

    public static readonly string SettingsBoxName = "UserNotificationsService";

    public static readonly IntSettingMediator WorkerTimePeriod = new IntSettingMediator("WorkerTimePeriod", 300, GetServiceSettingsBox);

    private static ISettingsBox _serviceSettingsBox;

    #endregion

    #region Public Properties

    public static ISettingsBox ServiceSettingsBox
    {
      get
      {
        if (_serviceSettingsBox != null)
        {
          return _serviceSettingsBox;
        }
        _serviceSettingsBox =
          HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox("PlantServices").GetSubBox(SettingsBoxName);
        return _serviceSettingsBox;
      }
    }

    #endregion

    #region Methods

    private static ISettingsBox GetServiceSettingsBox()
    {
      return ServiceSettingsBox;
    }

    #endregion
  }
}