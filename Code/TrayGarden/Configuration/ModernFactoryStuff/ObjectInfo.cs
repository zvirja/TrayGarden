using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff;

public class ObjectInfo
{
  public ObjectInfo(object instance, XmlNode configurationNode, bool isSingleton, bool isPrototype)
  {
    Instance = instance;
    ConfigurationNode = configurationNode;
    IsPrototype = isPrototype;
    IsSingleton = isSingleton;
  }

  public XmlNode ConfigurationNode { get; set; }

  public object Instance { get; set; }

  public bool IsPrototype { get; set; }

  public bool IsSingleton { get; set; }
}