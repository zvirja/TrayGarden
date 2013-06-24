using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.ClipboardObserver.Core;
using TrayGarden.Services.PlantServices.CustomSettings.Smorgasbord;

namespace TrayGarden.Services.PlantServices.CustomSettings.Core
{
    [UsedImplicitly]
    public class CustomSettingsService : PlantServiceBase<ClipboardObserverPlantBox>
    {
        public CustomSettingsService()
        {
            LuggageName = "CustomSettingsService";
        }


        public override void InitializePlant(IPlantEx plantEx)
        {
            SetCustomSettingsBox(plantEx);
        }


        protected virtual void SetCustomSettingsBox(IPlantEx plantEx)
        {
            var asExpected = plantEx.GetFirstWorkhorseOfType<IStoreCustomSettingsStorage>();
            if (asExpected == null)
                return;
            ISettingsBox settingsBox = plantEx.MySettingsBox.GetSubBox(LuggageName);
            asExpected.StoreCustomSettingsStorage(settingsBox);

            //Store luggage
            var luggage = new CustomSettingsServicePlantBox
                {
                    RelatedPlantEx = plantEx,
                    SettingsBox = settingsBox,
                    IsEnabled = true,
                };
            plantEx.PutLuggage(LuggageName, luggage);
        }
    }
}
