using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

[Obsolete]
public interface IUserSettingsBridgeMaster : IUserSettingsBridge
{
  void FakeRaiseSettingChange(IUserSetting oldValue, IUserSetting newValue);
  void Initialize(IEnumerable<IUserSettingMaster> userSettings, IUserSettingChangedStrategy defaultNotifyingStrategy);
  void RaiseSettingsChangedEvent(List<IUserSettingChange> changes);
}