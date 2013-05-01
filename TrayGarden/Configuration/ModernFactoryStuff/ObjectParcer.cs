using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public class ObjectParcer : IParcer
    {
        public static IParcer Instance { get; protected set; }

        static ObjectParcer()
        {
            Instance = new ObjectParcer();
        }


        public virtual object ParceNodeValue(XmlNode nodeValue)
        {
            var instance = ModernFactory.Instance.GetObject(nodeValue);
            return instance;
        }
    }
}