using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.Shortening;

public class ShortenerProvider : INeedCongurationNode
{
  protected XmlHelper ConfigurationHelper;

  public virtual string ApiKey
  {
    get
    {
      return this.ConfigurationHelper.GetStringValue("ApiKey", string.Empty);
    }
  }

  public string Name { get; set; }

  public void SetConfigurationNode(XmlNode configurationNode)
  {
    this.ConfigurationHelper = new XmlHelper(configurationNode);
  }

  public virtual bool TryShortUrl(string originalUrl, out string shortedUrl)
  {
    shortedUrl = originalUrl;
    return true;
  }
}