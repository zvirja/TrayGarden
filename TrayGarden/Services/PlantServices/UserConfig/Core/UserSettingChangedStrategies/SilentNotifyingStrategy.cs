using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies
{
    public class SilentNotifyingStrategy : ImpatientNotifyingStrategy
    {
        public override void NotifySettingChanged(IUserSetting before, IUserSetting after, IUserSettingsBridgeMaster originator)
        {
            
        }
    }
}