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
      return ConfigurationHelper.GetBoolValue("EnableConfirmation", true);
    }
  }

  protected bool EnableReverting
  {
    get
    {
      return ConfigurationHelper.GetBoolValue("EnableReverting", true);
    }
  }

  protected bool Enabled
  {
    get
    {
      return EnabledSetting.Value;
    }
  }

  protected IBoolUserSetting EnabledSetting { get; set; }

  protected UIDialogConfirmator ExecuteConfirmator { get; set; }

  protected UIDialogConfirmator RevertConfirmator { get; set; }

  public override void PostInit()
  {
    base.PostInit();
    EnabledSetting = DeclareEnabledSetting();
    if (EnableConfirmation)
    {
      ExecuteConfirmator = new UIDialogConfirmator(GetExecuteConfirmatorSettingName(), GetConfirmationDialog);
    }
    if (EnableReverting)
    {
      RevertConfirmator = new UIDialogConfirmator(GetRevertConfirmatorSettingName(), GetRevertDialog);
    }
  }

  //Use it to initialize handler

  public override bool PostmortemRevertValue(string currentUrl, string originalUrl, bool isClipboardRequest)
  {
    if (!isClipboardRequest)
    {
      return false;
    }
    if (RevertConfirmator != null)
    {
      return RevertConfirmator.ConfirmThroughUI() == true;
    }
    return base.PostmortemRevertValue(currentUrl, originalUrl, isClipboardRequest);
  }

  public override bool PreExecute(string operableUrl, bool isClipboardRequest)
  {
    if (!isClipboardRequest)
    {
      return true;
    }
    if (!Enabled)
    {
      return false;
    }
    if (ExecuteConfirmator != null)
    {
      return ExecuteConfirmator.ConfirmThroughUI() == true;
    }
    return base.PreExecute(operableUrl, isClipboardRequest);
  }

  protected virtual IBoolUserSetting DeclareEnabledSetting()
  {
    string settingNameAndTitle = GetEnabledSettingName();
    return UIConfigurationManager.ActualManager.SettingsSteward.DeclareBoolSetting(settingNameAndTitle, settingNameAndTitle, true);
  }

  protected virtual IResultProvider GetConfirmationDialog()
  {
    return new YesNoNotificationVM("Process clipboard value?");
  }

  protected virtual string GetEnabledSettingName()
  {
    return "Enable " + Name;
  }

  protected virtual string GetExecuteConfirmatorSettingName()
  {
    return "Confirm {0} execution".FormatWith(Name);
  }

  protected virtual string GetRevertConfirmatorSettingName()
  {
    return "Enable {0} reverting dialog".FormatWith(Name);
  }

  protected virtual IResultProvider GetRevertDialog()
  {
    return new YesNoNotificationVM("Revert processed value?");
  }
}