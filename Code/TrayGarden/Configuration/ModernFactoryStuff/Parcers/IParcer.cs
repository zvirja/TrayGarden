using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers;

public interface IParcer
{
  object ParceNodeValue(XmlNode nodeValue);
}