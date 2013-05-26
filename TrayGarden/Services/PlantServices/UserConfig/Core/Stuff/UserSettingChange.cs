using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public struct UserSettingChange : IUserSettingChange
    {
        public UserSettingChange(IUserSetting newUserSetting, IUserSetting oldUserSetting):this()
        {
            NewUserSetting = newUserSetting;
            OldUserSetting = oldUserSetting;
        }

        public IUserSetting NewUserSetting { get; protected set; }
        public IUserSetting OldUserSetting { get; protected set; }
    }
}