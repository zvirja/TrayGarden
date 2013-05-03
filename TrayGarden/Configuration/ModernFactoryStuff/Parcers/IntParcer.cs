using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public class IntParcer : IParcer
    {
        public static IParcer Instance { get; protected set; }

        static IntParcer()
        {
            Instance = new IntParcer();
        }

        public virtual object ParceNodeValue(XmlNode nodeValue)
        {
            string value = nodeValue.InnerText;
            int intValue;
            if (int.TryParse(value, out intValue))
                return intValue;
            return null;
        }
    }
}