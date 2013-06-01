using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher
{
    public class Mapping : IMapping
    {
        public IObjectFactory ObjectFactory { get; protected set; }
        public Type InterfaceType { get; protected set; }

        public virtual void Initialize([NotNull] string interfaceType, IObjectFactory objectFactory)
        {
            Assert.ArgumentNotNullOrEmpty(interfaceType, "interfaceType");
            Assert.ArgumentNotNull(objectFactory, "objectFactory");
            InterfaceType = ReflectionHelper.ResolveType(interfaceType);
            ObjectFactory = objectFactory;
        }

        public override string ToString()
        {
            if (ObjectFactory == null || InterfaceType == null)
                return base.ToString();
            return "Mapping: Interface {0}, ObjFactory {1}".FormatWith(InterfaceType.FullName,
                                                                       ObjectFactory.GetType().FullName);
        }
    }
}