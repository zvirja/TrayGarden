using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Stuff
{
    public delegate void UserSettingValueChanged(IUserSetting stateBefore, IUserSetting stateAfter);
}
