using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipboardChangerPlant.UIConfiguration;

using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

namespace ClipboardChangerPlant.RequestHandling;

public class RequestHandlerWithUIConfirmation : RequestHandler
{
  protected bool EnableConfirmation
  {
    get
    {
      return this.ConfigurationHelper.GetBoolValue("EnableConfirmation", true);
    }
  }

  protected bool EnableReverting
  {
    get
    {
      return this.ConfigurationHelper.GetBoolValue("EnableReverting", true);
    }
  }

  protected bool Enabled
  {
    get
    {
      return this.EnabledSetting.Value;
    }
  }

  protected IBoolUserSetting EnabledSetting { get; set; }

  protected UIDialogConfirmator ExecuteConfirmator { get; set; }

  protected UIDialogConfirmator RevertConfirmator { get; set; }

  public override void PostInit()
  {
    base.PostInit();
    this.EnabledSetting = this.DeclareEnabledSetting();
    if (this.EnableConfirmation)
    {
      this.ExecuteConfirmator = new UIDialogConfirmator(this.GetExecuteConfirmatorSettingName(), this.GetConfirmationDialog);
    }
    if (this.EnableReverting)
    {
      this.RevertConfirmator = new UIDialogConfirmator(this.GetRevertConfirmatorSettingName(), this.GetRevertDialog);
    }
  }

  //Use it to initialize handler

  public override bool PostmortemRevertValue(string currentUrl, string originalUrl, bool isClipboardRequest)
  {
    if (!isClipboardRequest)
    {
      return false;
    }
    if (this.RevertConfirmator != null)
    {
      return this.RevertConfirmator.ConfirmThroughUI() == true;
    }
    return base.PostmortemRevertValue(currentUrl, originalUrl, isClipboardRequest);
  }

  public override bool PreExecute(string operableUrl, bool isClipboardRequest)
  {
    if (!isClipboardRequest)
    {
      return true;
    }
    if (!this.Enabled)
    {
      return false;
    }
    if (this.ExecuteConfirmator != null)
    {
      return this.ExecuteConfirmator.ConfirmThroughUI() == true;
    }
    return base.PreExecute(operableUrl, isClipboardRequest);
  }

  protected virtual IBoolUserSetting DeclareEnabledSetting()
  {
    string settingNameAndTitle = this.GetEnabledSettingName();
    return UIConfigurationManager.ActualManager.SettingsSteward.DeclareBoolSetting(settingNameAndTitle, settingNameAndTitle, true);
  }

  protected virtual IResultProvider GetConfirmationDialog()
  {
    return new YesNoNotificationVM("Process clipboard value?");
  }

  protected virtual string GetEnabledSettingName()
  {
    return "Enable " + this.Name;
  }

  protected virtual string GetExecuteConfirmatorSettingName()
  {
    return "Confirm {0} execution".FormatWith(this.Name);
  }

  protected virtual string GetRevertConfirmatorSettingName()
  {
    return "Enable {0} reverting dialog".FormatWith(this.Name);
  }

  protected virtual IResultProvider GetRevertDialog()
  {
    return new YesNoNotificationVM("Revert processed value?");
  }
}