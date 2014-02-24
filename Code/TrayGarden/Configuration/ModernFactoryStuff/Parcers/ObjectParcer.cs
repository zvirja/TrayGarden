using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff.Parcers
{
    public class ObjectParcer : IParcer
    {
        protected ModernFactory ModernFactoryInstance { get; set; }

        public ObjectParcer(ModernFactory modernFactoryInstance)
        {
            ModernFactoryInstance = modernFactoryInstance;
        }

        public virtual object ParceNodeValue(XmlNode nodeValue)
        {
            var instance = ModernFactoryInstance.GetObject(nodeValue);
            return instance;
        }
    }
}