using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.RequestHandling
{
  public class RequestHandler : INeedCongurationNode
  {
    protected XmlHelper ConfigurationHelper;

    public virtual bool Match(string inputValue)
    {
      return MatchRegularExpressions.Any(matchRegularExpression => Regex.Match(inputValue, matchRegularExpression).Success);
    }

    public virtual bool IsShorterEnabled
    {
      get { return ConfigurationHelper.GetBoolValue("ShouldBeShorted"); }
    }

    public virtual bool ShoudBeConfirmed
    {
      get { return ConfigurationHelper.GetBoolValue("ConfirmBefore"); }
    }

    public virtual string[] MatchRegularExpressions
    {
      get { return ConfigurationHelper.GetStringValue("MatchRegExpressions").Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries); }
    }

    public virtual Icon HandlerIcon
    {
      get { return ResourcesOperator.GetIconByName(ConfigurationHelper.GetStringValue("SuccessIconResourceName")); }
    }

    public virtual bool TryProcess(string inputValue, out string result)
    {
      result = inputValue;
      return true;
    }

    public virtual void SetConfigurationNode(XmlNode configurationNode)
    {
      ConfigurationHelper = new XmlHelper(configurationNode);
    }

    public string Name { get; set; }

    public virtual void PreInit()
    {
      
    }

    public virtual void PostInit()
    {
      
    }
  }
}

