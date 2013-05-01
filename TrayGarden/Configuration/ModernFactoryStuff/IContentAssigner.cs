using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TrayGarden.Configuration.ModernFactoryStuff
{
    public interface IContentAssigner
    {
        void AssignContent(XmlNode contentNode, object instance, Type instanceType,
                           Func<Type, IParcer> valueParcerResolver);
    }
}