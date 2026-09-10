using System;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

[Obsolete]
public interface IUserSettingChangedStrategy
{
    void NotifySettingChanged([CanBeNull] IUserSetting before, IUserSetting after, IUserSettingsBridgeMaster originator);
}