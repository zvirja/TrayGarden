using System.Xml;
using System.Collections.Generic;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public class BoolParcer : IParcer
    {
        protected static Dictionary<ModernFactory, IParcer> Parcers { get; set; }

        public static BoolParcer()
        {
            Parcers = new Dictionary<ModernFactory, IParcer>();
        }

        public static IParcer GetParcer(ModernFactory factoryInstance)
        {
            if(
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