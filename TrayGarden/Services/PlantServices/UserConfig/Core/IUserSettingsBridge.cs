using System.Collections.Generic;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public interface IUserSettingsBridge
    {
        event UserSettingValuesChanged ValueChanged;

        IUserSetting GetUserSetting(string name);
        List<IUserSetting> GetUserSettings();
    }
}