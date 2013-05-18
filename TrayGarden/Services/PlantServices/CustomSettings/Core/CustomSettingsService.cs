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

        protected virtual void SetCustomSettingsBox(IPlantInternal plantInternal)
        {
            var asExpected = plantInternal.Workhorse as ISetCustomSettingsStorage;
            if (asExpected == null)
                return;
            ISettingsBox settingsBox = plantInternal.MySettingsBox.GetSubBox(LuggageName);
            asExpected.SetCustomSettingsStorage(settingsBox);
        }

        public virtual void InitializePlant(IPlantInternal plantInternal)
        {
            SetCustomSettingsBox(plantInternal);
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
