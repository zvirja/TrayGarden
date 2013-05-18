using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.CustomSettings.Smorgasbord;

namespace TrayGarden.Services.PlantServices.CustomSettings.Core
{
    [UsedImplicitly]
    public class CustomSettingsService : IService
    {
        public virtual string LuggageName { get; set; }

        public CustomSettingsService()
        {
            LuggageName = "CustomSettingsService";
        }

        protected virtual void SetCustomSettingsBox(IPlant plant)
        {
            var asExpected = plant.Workhorse as ISetCustomSettingsStorage;
            if (asExpected == null)
                return;
            ISettingsBox settingsBox = plant.MySettingsBox.GetSubBox(LuggageName);
            asExpected.SetCustomSettingsStorage(settingsBox);
        }

        public virtual void InitializePlant(IPlant plant)
        {
            SetCustomSettingsBox(plant);
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
    }
}
