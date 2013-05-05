using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.TypesHatcher
{
    public class Egg : ISupportPrototyping
    {
        protected string InstanceConfigurationNodePath { get; set; }

        protected object Instance { get; set; }
        public bool IsSingleton { get; protected set; }
        public Type KeyInterface { get; protected set; }
        protected bool SupportQuickInstantiation { get; set; }
        protected bool IsInitialized { get; set; }
        protected bool Erroneous { get; set; }


        public void InitializeByValues(string instanceConfigurationNodePath, Type keyInterface, bool isSingleton)
        {
            KeyInterface = keyInterface;
            IsSingleton = isSingleton;
            Instance = null;
            InstanceConfigurationNodePath = instanceConfigurationNodePath;
            SupportQuickInstantiation = false;
        }

        protected virtual void LateInitialization()
        {
            if (IsInitialized)
                return;
            IsInitialized = true;
            Instance = CreateNewObject();
            if (Instance == null)
            {
                Erroneous = true;
                return;
            }
            if(!KeyInterface.IsInstanceOfType(Instance))
            {
                Erroneous = true;
                return;
            }
            SupportQuickInstantiation = Instance is ISupportPrototyping;
        }

        protected virtual object ResolveInstance()
        {
            if (IsSingleton)
                return Instance;
            return CreateNewObject();
        }

        protected virtual object CreateNewObject()
        {
            if (SupportQuickInstantiation)
                return ((ISupportPrototyping) Instance).CreateNewInializedInstance();
            return Factory.Instance.GetObject(InstanceConfigurationNodePath);
        }

        public virtual object GetInstance()
        {
            if (!IsInitialized)
                LateInitialization();
            if (Erroneous)
                return null;
            return ResolveInstance();
        }

        public virtual object CreateNewInializedInstance()
        {
            return new Egg();
        }

        public object GetNewInstance()
        {
            if (!IsInitialized)
                LateInitialization();
            if (Erroneous)
                return null;
            if (!IsSingleton)
                return CreateNewObject();
            return null;
        }
    }
}