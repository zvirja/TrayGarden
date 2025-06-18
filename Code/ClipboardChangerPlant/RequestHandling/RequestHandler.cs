using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

using ClipboardChangerPlant.Configuration;
using ClipboardChangerPlant.RequestHandling.PipelineModel;

namespace ClipboardChangerPlant.RequestHandling;

public class RequestHandler : INeedCongurationNode
{
  protected XmlHelper ConfigurationHelper;

  public virtual Icon DefaultHandlerIcon
  {
    get
    {
      return ResourcesOperator.GetIconByName(this.ConfigurationHelper.GetStringValue("SuccessIconResourceName", "klipperSuccess"));
    }
  }

  public virtual bool IsShorterEnabled
  {
    get
    {
      return this.ConfigurationHelper.GetBoolValue("ShouldBeShorted", false);
    }
  }

  public string Name { get; set; }

  public virtual string[] RegularExpressionsToMatch
  {
    get
    {
      return this.ConfigurationHelper.GetStringValue("MatchRegExpressions", string.Empty)
        .Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
    }
  }

  public virtual bool? Match(ProcessorArgs args)
  {
    string inputValue = args.ResultUrl;
    return this.RegularExpressionsToMatch.Any(matchRegularExpression => Regex.Match(inputValue, matchRegularExpression).Success);
  }

  public virtual bool PostExecute(string operableUrl, bool isClipboardRequest)
  {
    return true;
  }

  public virtual void PostInit()
  {
  }

  public virtual bool PostmortemRevertValue(string currentUrl, string originalUrl, bool isClipboardRequest)
  {
    return false;
  }

  public virtual bool PreExecute(string operableUrl, bool isClipboardRequest)
  {
    return true;
  }

  public virtual void PreInit()
  {
  }

  public virtual void SetConfigurationNode(XmlNode configurationNode)
  {
    this.ConfigurationHelper = new XmlHelper(configurationNode);
  }

  public virtual bool TryProcess(string inputValue, out string result)
  {
    result = inputValue;
    return true;
  }
}