using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher
{
    public class Mapping : IMapping, IRequireInitialization
    {
        public string InterfaceTypeStr { get; set; }
        public string InstanceConfigurationPath { get; set; }
        public bool IsSingleton { get; set; }

        public Type InterfaceType { get; protected set; }


        public virtual void Initialize()
        {
            if (InterfaceTypeStr.NotNullNotEmpty())
                InterfaceType = ReflectionHelper.ResolveType(InterfaceTypeStr);
        }
    }
}