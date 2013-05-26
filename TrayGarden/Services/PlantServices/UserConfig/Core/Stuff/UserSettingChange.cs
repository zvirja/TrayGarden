using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Stuff
{
    public struct UserSettingChange : IUserSettingChange
    {
        public UserSettingChange(IUserSetting newUserSetting, IUserSetting oldUserSetting):this()
        {
            NewUserSetting = newUserSetting;
            OldUserSetting = oldUserSetting;
        }

        public IUserSetting NewUserSetting { get; private set; }
        public IUserSetting OldUserSetting { get; private set; }
    }
}