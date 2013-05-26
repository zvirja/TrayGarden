using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public class UserConfigServicePlantBox
    {
        public ISettingsBox SettingsBox { get; set; }
        public IPlantEx PlantEx { get; set; }
        public IUserSettingsBridgeMaster SettingsBridge { get; set; }
    }
}
