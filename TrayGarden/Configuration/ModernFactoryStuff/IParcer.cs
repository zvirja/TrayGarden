using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public interface IParcer
    {
        object ParceNodeValue(XmlNode nodeValue);
    }
}