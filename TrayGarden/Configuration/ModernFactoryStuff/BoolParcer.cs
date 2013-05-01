using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public class BoolParcer : IParcer
    {
        public static IParcer Instance { get; protected set; }

        static BoolParcer()
        {
            Instance = new BoolParcer();
        }

        public virtual object ParceNodeValue(XmlNode nodeValue)
        {
            string value = nodeValue.InnerText;
            bool intValue;
            if (bool.TryParse(value, out intValue))
                return intValue;
            return null;
        }
    }
}