using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher
{
    public class Egg : IPrototype
    {
        protected string InstanceConfigurationNodePath { get; set; }

        protected object Instance { get; set; }
        public bool IsSingleton { get; protected set; }
        public Type KeyInterface { get; protected set; }
        protected bool SupportQuickInstantiation { get; set; }


        public void InitializeByValues(string instanceConfigurationNodePath, Type keyInterface, bool isSingleton,
                                       object instance)
        {
            KeyInterface = keyInterface;
            IsSingleton = isSingleton;
            Instance = instance;
            InstanceConfigurationNodePath = instanceConfigurationNodePath;
            SupportQuickInstantiation = instance is IPrototype;
        }

        protected virtual object ResolveNewObject()
        {
            if (IsSingleton)
                return Instance;
            return CreateNewObject();
        }

        protected virtual object CreateNewObject()
        {
            if (SupportQuickInstantiation)
                return ((IPrototype) Instance).CreateNewInializedInstance();
            return ModernFactory.Instance.GetObject(InstanceConfigurationNodePath);
        }

        public virtual object GetInstance()
        {
            if (IsSingleton)
                return Instance;
            return ResolveNewObject();
        }

        public virtual object CreateNewInializedInstance()
        {
            return new Egg();
        }

        public object GetNewInstance()
        {
            if (!IsSingleton)
                return CreateNewObject();
            return null;
        }
    }
}