using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public class UserConfigServicePlantBox : ServicePlantBoxBase
    {
        public IUserSettingsBridgeMaster SettingsBridge { get; set; }
    }
}
