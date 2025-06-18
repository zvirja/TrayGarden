using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers;

public class StringParcer : IParcer
{
  public virtual object ParceNodeValue(XmlNode nodeValue)
  {
    var value = nodeValue.InnerText;
    return value;
  }
}