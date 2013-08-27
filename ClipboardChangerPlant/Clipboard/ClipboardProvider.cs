using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using ClipboardChangerPlant.Configuration;

namespace ClipboardChangerPlant.Clipboard
{
  public class ClipboardProvider
  {
    protected XmlHelper ConfigurationHelper;

    public string Name { get; set; }


    public virtual string GetValue()
    {
      return System.Windows.Clipboard.GetText();
    }

    public virtual void SetValue(string value)
    {
      System.Windows.Clipboard.SetText(value);
    }

    public void SetConfigurationNode(XmlNode configurationNode)
    {
      ConfigurationHelper = new XmlHelper(configurationNode);
    }
  }
}
