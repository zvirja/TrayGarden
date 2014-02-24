using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.Shortening
{
  public class ShortenerProvider : INeedCongurationNode
  {
    protected XmlHelper ConfigurationHelper;

    public virtual bool TryShortUrl(string originalUrl, out string shortedUrl)
    {
      shortedUrl = originalUrl;
      return true;
    }

    public virtual string ApiKey
    {
      get { return ConfigurationHelper.GetStringValue("ApiKey", string.Empty); }
    }

    public void SetConfigurationNode(XmlNode configurationNode)
    {
      ConfigurationHelper = new XmlHelper(configurationNode);
    }

    public string Name { get; set; }
  }
}
