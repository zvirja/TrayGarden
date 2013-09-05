using System;
using System.Windows.Markup;
using System.Xml;
using ClipboardChangerPlant.UIConfiguration;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

namespace ClipboardChangerPlant.RequestHandling
{
  public class RequestHandlerWithUIConfirmation : RequestHandler
  {
    protected bool EnableConfirmation
    {
      get { return ConfigurationHelper.GetBoolValue("EnableConfirmation", true); }
    }

    protected bool EnableReverting
    {
      get { return ConfigurationHelper.GetBoolValue("EnableReverting", true); }
    }
    
    protected bool Enabled
    {
      get
      {
        IUserSetting enabledSetting = UIConfigurationManager.ActualManager.UserSettingsBridge.GetUserSetting(GetEnabledSettingName());
        if (enabledSetting == null)
          return false;
        return enabledSetting.BoolValue;
      }
    }

    protected UIDialogConfirmator ExecuteConfirmator { get; set; }

    protected UIDialogConfirmator RevertConfirmator { get; set; }
    //Use it to initialize handler

    public override void SetConfigurationNode(XmlNode configurationNode)
    {
      base.SetConfigurationNode(configurationNode);
      UIConfigurationManager.ActualManager.VolatileUserSettings.Add(AddEnablingSetting);
      if (EnableConfirmation)
        ExecuteConfirmator = new UIDialogConfirmator(GetExecuteConfirmatorSettingName(), GetConfirmationDialog);
      if (EnableReverting)
        RevertConfirmator = new UIDialogConfirmator(GetRevertConfirmatorSettingName(), GetRevertDialog);
    }

    public override bool PreExecute(string operableUrl, bool isClipboardRequest)
    {
      if (!Enabled)
        return false;
      if (ExecuteConfirmator != null)
        return ExecuteConfirmator.ConfirmThroughUI() == true;
      return base.PreExecute(operableUrl, isClipboardRequest);
    }

    public override bool PostmortemRevertValue(string currentUrl, string originalUrl, bool isClipboardRequest)
    {
      if (RevertConfirmator != null)
        return RevertConfirmator.ConfirmThroughUI() == true;
      return base.PostmortemRevertValue(currentUrl, originalUrl, isClipboardRequest);
    }

    protected virtual void AddEnablingSetting(IUserSettingsMetadataBuilder builder)
    {
      builder.DeclareBoolSetting(GetEnabledSettingName(), true);
    }

    protected virtual IResultProvider GetRevertDialog()
    {
      return new YesNoNotificationVM("Revert processed value?");
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
  }
}