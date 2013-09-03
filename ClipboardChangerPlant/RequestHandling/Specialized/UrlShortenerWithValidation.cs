using System;
using System.Windows.Markup;
using System.Xml;
using ClipboardChangerPlant.UIConfiguration;
using JetBrains.Annotations;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.SpecializedNotifications.ViewModes;

namespace ClipboardChangerPlant.RequestHandling.Specialized
{
  [UsedImplicitly]
  public class UrlShortenerWithValidation : RequestHandler
  {
    protected UIDialogConfirmator ExecuteConfirmator { get; set; }
    protected UIDialogConfirmator RevertConfirmator { get; set; }
    protected UIDialogConfirmator ExecuteChecker { get; set; }

    protected string OperableUrlValue { get; set; }



    protected virtual bool ConfirmBefore
    {
      get { return ConfigurationHelper.GetBoolValue("ConfirmBefore"); }
    }

    protected virtual bool ShowRevertOption
    {
      get { return ConfigurationHelper.GetBoolValue("ShowRevertOption"); }
    }


    public override bool PreExecute(string operableUrl, bool isClipboardRequest)
    {
      OperableUrlValue = operableUrl;
      if (!ExecuteChecker.ConfirmationSetting.BoolValue)
        return false;
      if (ExecuteConfirmator == null)
        return true;
      if (ExecuteConfirmator.ConfirmThroughUI() != true)
        return false;
      return base.PreExecute(operableUrl, isClipboardRequest);
    }


    public override bool PostmortemRevertValue(string currentUrl, string originalUrl, bool isClipboardRequest)
    {
      if (RevertConfirmator == null)
        return false;
      if (RevertConfirmator.ConfirmThroughUI() == true)
        return true;
      return base.PostmortemRevertValue(currentUrl, originalUrl, isClipboardRequest);
    }

    public override void SetConfigurationNode(XmlNode configurationNode)
    {
      base.SetConfigurationNode(configurationNode);
      ExecuteChecker = RegisterUIDialogConfirmator("Enable {0}".FormatWith(Name),() => null);
      if(ConfirmBefore)
        ExecuteConfirmator = RegisterUIDialogConfirmator("Confirm {0} execution".FormatWith(Name), GetConfirmationDialog);
      if (ShowRevertOption)
        RevertConfirmator = RegisterUIDialogConfirmator("Enable {0} reverting dialog".FormatWith(Name), GetRevertDialog);
    }

    protected virtual IResultProvider GetConfirmationDialog()
    {
      return new YesNoNotificationVM("Short url?{0}{1}".FormatWith(Environment.NewLine, OperableUrlValue));
    }

    protected virtual IResultProvider GetRevertDialog()
    {
      return new YesNoNotificationVM("Revert shortening?{0}{1}".FormatWith(Environment.NewLine, OperableUrlValue));

    }
  }
}
