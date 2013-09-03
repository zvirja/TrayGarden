using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text.RegularExpressions;
using System.Xml;
using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.RequestHandling.PipelineModel;
using ClipboardChangerPlant.UIConfiguration;
using TrayGarden.Services.PlantServices.UserNotifications.Core.UI.ResultDelivering;

namespace ClipboardChangerPlant.RequestHandling
{
  public class RequestHandler : INeedCongurationNode
  {
    protected XmlHelper ConfigurationHelper;
    protected List<UIDialogConfirmator> Confirmators { get; set; }

    public RequestHandler()
    {
      Confirmators = new List<UIDialogConfirmator>();
    }

    public virtual bool? Match(ProcessorArgs args)
    {
      string inputValue = args.ResultUrl;
      return MatchRegularExpressions.Any(matchRegularExpression => Regex.Match(inputValue, matchRegularExpression).Success);
    }

    public virtual bool IsShorterEnabled
    {
      get { return ConfigurationHelper.GetBoolValue("ShouldBeShorted"); }
    }

    public virtual string[] MatchRegularExpressions
    {
      get { return ConfigurationHelper.GetStringValue("MatchRegExpressions").Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries); }
    }

    public virtual Icon HandlerIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("SuccessIconResourceName")); }
    }

    public virtual bool PreExecute(string operableUrl, bool isClipboardRequest)
    {
      return true;
    }

    public virtual bool TryProcess(string inputValue, out string result)
    {
      result = inputValue;
      return true;
    }

    public virtual bool PostExecute(string operableUrl, bool isClipboardRequest)
    {
      return true;
    }

    public virtual bool PostmortemRevertValue(string currentUrl, string originalUrl, bool isClipboardRequest)
    {
      return false;
    }

    public virtual void SetConfigurationNode(XmlNode configurationNode)
    {
      ConfigurationHelper = new XmlHelper(configurationNode);
    }

    public string Name { get; set; }

    public virtual void PreInit()
    {
      foreach (UIDialogConfirmator confirmator in Confirmators)
        confirmator.PreInit();
    }

    public virtual void PostInit()
    {
      foreach (UIDialogConfirmator confirmator in Confirmators)
        confirmator.PostInit();
    }

    protected UIDialogConfirmator RegisterUIDialogConfirmator(string confirmationSettingName, Func<IResultProvider> uiDialogConstructor)
    {
      var uiConfirmator = new UIDialogConfirmator(confirmationSettingName, uiDialogConstructor);
      Confirmators.Add(uiConfirmator);
      return uiConfirmator;
    }

  }
}

