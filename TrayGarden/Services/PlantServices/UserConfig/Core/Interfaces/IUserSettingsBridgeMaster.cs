using System.Collections.Generic;
using TrayGarden.Plants;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingsBridgeMaster : IUserSettingsBridge
    {
        void Initialize(IEnumerable<IUserSettingMaster> userSettings, IUserSettingChangedStrategy defaultNotifyingStrategy);
        void RaiseSettingsChangedEvent(List<IUserSettingChange> changes);
        void FakeRaiseSettingChange(IUserSetting oldValue, IUserSetting newValue);
    }
}