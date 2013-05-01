using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher
{
    public class HatcherManager : IRequireInitialization
    {
        protected static readonly Lazy<HatcherManager> Instance =
            new Lazy<HatcherManager>(() => ModernFactory.Instance.GetObject<HatcherManager>("typeHatcherManager"));

        public static HatcherManager Actual
        {
            get { return Instance.Value; }
        }

        protected Dictionary<Type, Egg> Eggs { get; set; }

        public List<IMapping> Mappings { get; set; }
        public IPrototype EggPrototype { get; set; }

        public HatcherManager()
        {
            Mappings = new List<IMapping>();
            Eggs = new Dictionary<Type, Egg>();
        }

        public virtual void Initialize()
        {
            var eggPrototype = EggPrototype;
            if (eggPrototype == null)
                return;
            foreach (IMapping mapping in Mappings)
            {
                var resolvedEgg = ResolveEgg(mapping, eggPrototype);
                if (resolvedEgg != null)
                    Eggs.Add(resolvedEgg.KeyInterface, resolvedEgg);
            }
        }


        protected virtual Egg ResolveEgg(IMapping mapping, IPrototype eggPrototype)
        {
            var newEgg = (Egg) eggPrototype.CreateNewInializedInstance();
            if (TryInitializeFromMapping(mapping, newEgg))
                return newEgg;
            return null;
        }

        protected virtual bool TryInitializeFromMapping(IMapping mapping, Egg egg)
        {
            bool isSingleton = mapping.IsSingletone;
            Type interfaceType = mapping.InterfaceType;
            if (interfaceType == null)
                return false;
            if (!interfaceType.IsInterface)
                return false;
            string instanceConfigurationPath = mapping.InstanceConfigurationPath;
            if (instanceConfigurationPath.IsNullOrEmpty())
                return false;
            object instantiatedObject = ModernFactory.Instance.GetObject(instanceConfigurationPath);
            if (instantiatedObject == null)
                return false;
            if (!interfaceType.IsInstanceOfType(instantiatedObject))
                return false;
            egg.InitializeByValues(instanceConfigurationPath, interfaceType, isSingleton,
                                   instantiatedObject);
            return true;
        }


        public virtual object GetObjectByType(Type keyInterface)
        {
            if (Eggs.ContainsKey(keyInterface))
                return Eggs[keyInterface].GetInstance();
            return null;
        }

        public virtual object GetNewObjectByType(Type keyInterface)
        {
            if (Eggs.ContainsKey(keyInterface))
                return Eggs[keyInterface].GetNewInstance();
            return null;
        }
    }
}