using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  [Obsolete]
    public interface IUserSettingChangedStrategy
    {
        void NotifySettingChanged([CanBeNull] IUserSetting before, IUserSetting after, IUserSettingsBridgeMaster originator);
    }
}
