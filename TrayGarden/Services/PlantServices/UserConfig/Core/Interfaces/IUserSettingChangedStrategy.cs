using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingChangedStrategy
    {
        void NotifySettingChanged([CanBeNull] IUserSetting before, IUserSetting after, IUserSettingsBridgeMaster originator);
    }
}
