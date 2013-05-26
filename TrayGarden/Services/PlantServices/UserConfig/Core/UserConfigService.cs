using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies.Switchers;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    [UsedImplicitly]
    public class UserConfigService : PlantServiceBase<UserConfigServicePlantBox>
    {

        public UserConfigService()
        {
            LuggageName = "UserConfigService";
        }
        public override void InitializePlant(IPlantEx plantEx)
        {
            base.InitializePlant(plantEx);
            InitializePlantInternal(plantEx);
        }

        public override void InformDisplayStage()
        {
            base.InformDisplayStage();
            NotifyAllPlantBoxInitValues();
        }

        protected virtual void InitializePlantInternal(IPlantEx plantEx)
        {
            InitPlantUCPipeline.Run(LuggageName,plantEx);
        }

        /// <summary>
        /// Initial notifying. This is the only way to receive the initial values (restored from old session);
        /// </summary>
        protected virtual void NotifyAllPlantBoxInitValues()
        {
            List<UserConfigServicePlantBox> luggages =
    HatcherGuide<IGardenbed>.Instance.GetAllPlants().Select(GetPlantLuggage).Where(x => x != null).ToList();
            using (new AccumulativeNotifyingStrategySwitcher())
            {
                foreach (UserConfigServicePlantBox plantBox in luggages)
                {
                    var bridge = plantBox.SettingsBridge;
                    List<IUserSetting> userSettings = bridge.GetUserSettings();
                    foreach (IUserSetting userSetting in userSettings)
                    {
                        bridge.FakeRaiseSettingChange(null,userSetting);
                    }
                }
            }
        }
    }
}
