using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserNotifications.Core.Plants;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.Displaying;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace ClipboardChangerPlant.UIConfiguration
{
  public class UIDialogConfirmator
  {
    protected Func<IResultProvider> UIDialogConstructor { get; set; }

    public ILordOfNotifications LordOfNotifications { get; set; }
    public string ConfirmationSettingName { get; set; }
    public IUserSetting ConfirmationSetting { get; set; }

    public UIDialogConfirmator(string confirmationSettingName, Func<IResultProvider> uiDialogConstructor)
    {
      ConfirmationSettingName = confirmationSettingName;
      UIDialogConstructor = uiDialogConstructor;
    }


    public virtual void PreInit()
    {
      UIConfigurationManager.ActualManager.VolatileUserSettings.Add(PopulateConfirmationSetting);
    }

    public virtual void PostInit()
    {
      ConfirmationSetting = UIConfigurationManager.ActualManager.UserSettings[ConfirmationSettingName];
      LordOfNotifications = PopupDialogsManager.ActualManager.LordOfNotifications;
      
    }

    public virtual bool? ConfirmThroughUI()
    {
      if (!ConfirmationSetting.BoolValue)
        return null;
      return GetConfirmationFromUIDialog();
    }

    protected virtual void PopulateConfirmationSetting(IUserSettingsMetadataBuilder userSettingsMetadataBuilder)
    {
      userSettingsMetadataBuilder.DeclareBoolSetting(ConfirmationSettingName, true);
    }

    protected virtual bool? GetConfirmationFromUIDialog()
    {
      IResultProvider dialog = UIDialogConstructor();
      if (dialog == null)
        return null;
      INotificationResultCourier resultCourier = LordOfNotifications.DisplayNotification(dialog);
      NotificationResult result = resultCourier.GetResultWithWait();
      if (result.Code == ResultCode.OK || result.Code == ResultCode.Yes)
        return true;
      if (result.Code == ResultCode.Close || result.Code == ResultCode.No)
        return false;
      if (result.Code == ResultCode.PermanentlyClose)
        ConfirmationSetting.BoolValue = false;
      return null;
    }


  }
}
