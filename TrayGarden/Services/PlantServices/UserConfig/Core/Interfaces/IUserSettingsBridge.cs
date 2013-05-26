using System.Collections.Generic;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingsBridge
    {
        event UserSettingValuesChanged SettingValuesChanged;

        IUserSetting GetUserSetting(string name);
        List<IUserSetting> GetUserSettings();
    }
}