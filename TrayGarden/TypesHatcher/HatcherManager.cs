using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher
{
    public class HatcherManager
    {
        protected static readonly Lazy<HatcherManager> Instance =
            new Lazy<HatcherManager>(() => Factory.Instance.GetObject<HatcherManager>("typeHatcherManager"));

        public static HatcherManager Actual
        {
            get { return Instance.Value; }
        }

        protected bool Initialized { get; set; }
        protected Dictionary<Type, IObjectFactory> Mappings { get; set; }

        //protected IObjectFactory EggFactory { get; set; }

        public HatcherManager()
        {
            Mappings = new Dictionary<Type, IObjectFactory>();
        }

        public virtual void Initialize(List<IMapping> mappings)
        {
           // if (eggFactory == null)
               // throw new ArgumentNullException("eggFactory");
            if (mappings == null)
                throw new ArgumentNullException("mappings");
           // EggFactory = eggFactory;
            foreach (IMapping mapping in mappings)
            {
                if (ValidateMapping(mapping))
                    Mappings.Add(mapping.InterfaceType, mapping.ObjectFactory);
                //var resolvedEgg = ResolveEgg(mapping, eggFactory);
                //if (resolvedEgg != null)
                //    Eggs.Add(resolvedEgg.KeyInterface, resolvedEgg);
            }
            Initialized = true;
        }

        protected virtual bool ValidateMapping(IMapping mapping)
        {
            Type interfaceType = mapping.InterfaceType;
            if (interfaceType == null)
                return false;
            if (!interfaceType.IsInterface)
                return false;
            if (mapping.ObjectFactory == null)
                return false;
            return true;
        }

        /*protected virtual Egg ResolveEgg(IMapping mapping, IObjectFactory eggFactory)
        {
            var newEgg = (Egg)eggFactory.GetPurelyNewObject();
            if (TryInitializeFromMapping(mapping, newEgg))
                return newEgg;
            return null;
        }

        protected virtual bool TryInitializeFromMapping(IMapping mapping, Egg egg)
        {
            Type interfaceType = mapping.InterfaceType;
            if (interfaceType == null)
                return false;
            if (!interfaceType.IsInterface)
                return false;
            string instanceConfigurationPath = mapping.InstanceConfigurationPath;
            if (instanceConfigurationPath.IsNullOrEmpty())
                return false;
            egg.InitializeByValues(instanceConfigurationPath, interfaceType, isSingleton);
            return true;
        }*/


        public virtual object GetObjectByType(Type keyInterface)
        {
            if (!Initialized)
                throw new NonInitializedException();
            if (Mappings.ContainsKey(keyInterface))
                return Mappings[keyInterface].GetObject();
            return null;
        }

        public virtual object GetNewObjectByType(Type keyInterface)
        {
            if (!Initialized)
                throw new NonInitializedException();
            if (Mappings.ContainsKey(keyInterface))
                return Mappings[keyInterface].GetPurelyNewObject();
            return null;
        }
    }
}