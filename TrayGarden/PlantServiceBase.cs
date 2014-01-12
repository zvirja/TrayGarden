using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services
{
    public abstract class PlantServiceBase<TPlantLuggageType> : IService where TPlantLuggageType : class
    {
        protected static readonly string AllServiceSettingsContainerName = "PlantServices";
        protected ISettingsBox _serviceSettingsBox;
        protected bool? _isActuallyEnabled;


        protected PlantServiceBase(string serviceName, string luggageName)
        {
            ServiceName = serviceName;
            LuggageName = luggageName;
        }

        public string LuggageName { get; set; }
        public string ServiceName { get; protected set; }
        public string ServiceDescription { get; protected set; }
        public bool IsEnabled
        {
            get { return ServiceSettingsBox.GetBool("IsEnabled", true); }
            set
            {
                IsActuallyEnabled = IsEnabled;
                ServiceSettingsBox.SetBool("IsEnabled", value);
                OnIsEnabledChanged(value);
            }
        }

        public virtual bool CanBeDisabled
        {
            get { return true; }
        }

        public bool IsActuallyEnabled
        {
            get
            {
                if(_isActuallyEnabled != null)
                    return _isActuallyEnabled.Value;
                _isActuallyEnabled = IsEnabled;
                return _isActuallyEnabled.Value;

            }
            protected set
            {
                if(_isActuallyEnabled == null)
                    _isActuallyEnabled = value;
            }
        }

        public event Action<bool> IsEnabledChanged;

        protected virtual ISettingsBox ServiceSettingsBox
        {
            get
            {
                if(_serviceSettingsBox != null)
                    return _serviceSettingsBox;
                var key = GetType().Name;
                var settingsRootBox =
                    HatcherGuide<IRuntimeSettingsManager>.Instance.SystemSettings.GetSubBox(
                        AllServiceSettingsContainerName);
                _serviceSettingsBox = settingsRootBox.GetSubBox(key);
                return _serviceSettingsBox;
            }
        }


        public virtual TPlantLuggageType GetPlantLuggage(IPlantEx plantEx)
        {
            if (!plantEx.HasLuggage(LuggageName))
                return null;
            return plantEx.GetLuggage<TPlantLuggageType>(LuggageName);
        }

        protected virtual void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
        {

        }

        public virtual void InitializePlant(IPlantEx plantEx)
        {
            plantEx.EnabledChanged += PlantOnEnabledChanged;
        }

        public virtual void InformInitializeStage()
        {

        }

        public virtual void InformDisplayStage()
        {

        }

        public virtual void InformClosingStage()
        {

        }

        public virtual bool IsAvailableForPlant(IPlantEx plantEx)
        {
            return GetPlantLuggage(plantEx) != null;
        }

        protected virtual void OnIsEnabledChanged(bool obj)
        {
            Action<bool> handler = IsEnabledChanged;
            if (handler != null) handler(obj);
        }
    }
}
