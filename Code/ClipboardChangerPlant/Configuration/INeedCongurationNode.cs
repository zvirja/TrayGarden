using System.Xml;

namespace ClipboardChangerPlant.Configuration;

public interface INeedCongurationNode
{
  string Name { get; set; }

  void SetConfigurationNode(XmlNode configurationNode);
}