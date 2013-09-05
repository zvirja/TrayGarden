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
    protected IUserSetting confirmationSetting;
    protected Func<IResultProvider> UIDialogConstructor { get; set; }

    public ILordOfNotifications LordOfNotifications
    {
      get
      {
        return PopupDialogsManager.ActualManager.LordOfNotifications;
      }
    }
    public string ConfirmationSettingName { get; set; }

    public IUserSetting ConfirmationSetting
    {
      get
      {
        if (confirmationSetting != null)
          return confirmationSetting;
        confirmationSetting = UIConfigurationManager.ActualManager.UserSettings[ConfirmationSettingName];
        return confirmationSetting;
      }
    }

    public UIDialogConfirmator(string confirmationSettingName, Func<IResultProvider> uiDialogConstructor)
    {
      ConfirmationSettingName = confirmationSettingName;
      UIDialogConstructor = uiDialogConstructor;
      //Moved to constructor because believe that it will be created before the initialization happen
      UIConfigurationManager.ActualManager.VolatileUserSettings.Add(PopulateConfirmationSetting);
    }


    public virtual void PreInit()
    {
      UIConfigurationManager.ActualManager.VolatileUserSettings.Add(PopulateConfirmationSetting);
    }

    public virtual bool? ConfirmThroughUI()
    {
      if (ConfirmationSetting == null)
        return null;
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
