using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;
using TrayGarden.RuntimeSettings.Provider;
using TrayGarden.Helpers;

namespace TrayGarden.Plants
{
    [UsedImplicitly]
    public class PlantInternalInternal : IPlantInternal
    {
        public object Workhorse { get; set; }
        public string ID { get; protected set; }
        public ISettingsBox MySettingsBox { get; protected set; }

        public event PlantEnabledChangedEvent EnabledChanged;

       

        public bool IsEnabled
        {
            get
            {
                if (!IsInitialized)
                    throw new NonInitializedException();
                return MySettingsBox.GetBool("enabled", false);
            }
            set
            {
                if (!IsInitialized)
                    throw new NonInitializedException();
                MySettingsBox.SetBool("enabled", value);
                OnEnabledChanged(this, value);
            }
        }

        protected Dictionary<string, object> Cloakroom { get; set; }
        protected bool IsInitialized { get; set; }

        public PlantInternalInternal()
        {
            Cloakroom = new Dictionary<string, object>();
        }

        public virtual void Initialize(object workhorse, string id, ISettingsBox mySettingsBox)
        {
            if (workhorse == null) throw new ArgumentNullException("workhorse");
            if (mySettingsBox == null) throw new ArgumentNullException("mySettingsStorage");
            if (id.IsNullOrEmpty()) throw new ArgumentNullException("id");
            Workhorse = workhorse;
            ID = id;
            MySettingsBox = mySettingsBox;
            IsInitialized = true;
        }

        public virtual bool HasLuggage(string name)
        {
            return Cloakroom.ContainsKey(name);
        }

        public virtual object GetLuggage(string name)
        {
            if (!Cloakroom.ContainsKey(name))
                return null;
            return Cloakroom[name];
        }

        public virtual T GetLuggage<T>(string name) where T : class
        {
            return GetLuggage(name) as T;
        }

        public virtual void PutLuggage(string name, object luggage)
        {
            Cloakroom[name] = luggage;
        }

        protected virtual void OnEnabledChanged(IPlantInternal plantInternal, bool newValue)
        {
            PlantEnabledChangedEvent handler = EnabledChanged;
            if (handler != null) handler(plantInternal, newValue);
        }
    }
}
