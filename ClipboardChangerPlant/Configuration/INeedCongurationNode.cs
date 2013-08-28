using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ClipboardChangerPlant.Configuration
{
  public interface INeedCongurationNode
  {
    void SetConfigurationNode(XmlNode configurationNode);
    string Name { get; set; }
  }
}
