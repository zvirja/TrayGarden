using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies.Switchers
{
    public class ImpatientNotifyingStrategySwitcher : NotifyingStrategySwitcher
    {
        public ImpatientNotifyingStrategySwitcher()
            : base(new ImpatientNotifyingStrategy())
        {

        }
    }
}
