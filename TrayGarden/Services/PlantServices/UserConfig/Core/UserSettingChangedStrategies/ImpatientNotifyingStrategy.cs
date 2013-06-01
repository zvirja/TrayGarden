using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies
{
    public class ImpatientNotifyingStrategy : IUserSettingChangedStrategy
    {

        public virtual void NotifySettingChanged(IUserSetting before, [NotNull] IUserSetting after,
                                                 [NotNull] IUserSettingsBridgeMaster originator)
        {
            Assert.ArgumentNotNull(after, "after");
            Assert.ArgumentNotNull(originator, "originator");
            originator.RaiseSettingsChangedEvent(new List<IUserSettingChange>{GetChange(before,after)});
        }

        protected virtual IUserSettingChange GetChange(IUserSetting before, IUserSetting after)
        {
            return new UserSettingChange(after,before);
        }
    }
}