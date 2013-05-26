using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public delegate void UserSettingValueChanged(IUserSetting stateBefore, IUserSetting stateAfter);
}
