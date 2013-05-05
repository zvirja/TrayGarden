using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher
{
    public class Mapping : IMapping
    {
        public IObjectFactory ObjectFactory { get; protected set; }
        public Type InterfaceType { get; protected set; }

        public virtual void Initialize(string interfaceType, IObjectFactory objectFactory)
        {
            if (interfaceType.IsNullOrEmpty())
                throw new ArgumentNullException("interfaceType");
            if (objectFactory == null)
                throw new ArgumentNullException("objectFactory");
            InterfaceType = ReflectionHelper.ResolveType(interfaceType);
            ObjectFactory = objectFactory;
        }
    }
}