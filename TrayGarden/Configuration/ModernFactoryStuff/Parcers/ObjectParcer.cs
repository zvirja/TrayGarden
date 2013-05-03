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
            //TODO SOMEHOW REMOVE DEPENDENCE FROM MODERN FACTORY
            var instance = ModernFactory.ActualInstance.GetObject(nodeValue);
            return instance;
        }
    }
}