using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingMaster:IUserSetting
    {
        void Initialize(IUserSettingMetadata metadata, ISettingsBox containerSettingsBox);
    }
}
