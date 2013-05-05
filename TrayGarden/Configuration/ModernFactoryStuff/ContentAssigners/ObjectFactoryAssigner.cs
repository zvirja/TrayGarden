using System;
using System.Xml;
using TrayGarden.Configuration.ModernFactoryStuff.Parcers;

namespace TrayGarden.Configuration.ModernFactoryStuff.ContentAssigners
{
    public class ObjectFactoryAssigner : IContentAssigner
    {
        public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType,
                                          Func<Type, IParcer> valueParcerResolver)
        {
            var objectFactoryInstance = instance as ModernFactory.ObjectFactory;
            if (objectFactoryInstance == null)
                return;
            var contentNodeClone = contentNode.Clone();
            var typeAttribute = contentNodeClone.Attributes["type"];
            if (typeAttribute == null)
                return;
            var indexOfColon = typeAttribute.Value.IndexOf(":");
            if (indexOfColon == -1)
                return;
            var newTypeValue = typeAttribute.Value.Substring(indexOfColon + 1);
            typeAttribute.Value = newTypeValue;
            objectFactoryInstance.Initialize(contentNodeClone);
        }
    }
}