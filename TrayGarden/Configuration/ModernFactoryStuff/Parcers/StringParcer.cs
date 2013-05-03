using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public class StringParcer : IParcer
    {
        public static IParcer Instance { get; protected set; }

        static StringParcer()
        {
            Instance = new StringParcer();
        }

        public virtual object ParceNodeValue(XmlNode nodeValue)
        {
            var value = nodeValue.InnerText;
            return value;
        }
    }
}