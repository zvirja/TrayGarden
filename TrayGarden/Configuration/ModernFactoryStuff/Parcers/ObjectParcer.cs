using System.Xml;
using TrayGarden.Configuration.ModernFactoryStuff.Parcers;

namespace TrayGarden.Configuration.ModernFactoryStuff
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