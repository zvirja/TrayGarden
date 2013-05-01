using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Helpers;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public class SimpleAssigner : IContentAssigner
    {
        public static IContentAssigner Instance { get; protected set; }

        static SimpleAssigner()
        {
            Instance = new SimpleAssigner();
        }

        public virtual void AssignContent(XmlNode contentNode, object instance, Type instanceType,
                                          Func<Type, IParcer> valueParcerResolver)
        {
            var nodeName = contentNode.Name;
            XmlNode nodeValue = contentNode.FirstChild;
            if (nodeValue == null)
                return;
            var property = instanceType.GetProperty(nodeName);
            if (property == null)
                return;
            if (!property.CanWrite)
                return;
            var propertyType = property.PropertyType;
            IParcer parcer = valueParcerResolver(propertyType);
            if (parcer == null)
                return;
            object contentValue = parcer.ParceNodeValue(nodeValue);
            if (contentValue == null)
                return;
            if (!propertyType.IsInstanceOfType(contentValue))
                return;
            property.SetValue(instance, contentValue, null);
        }
    }
}