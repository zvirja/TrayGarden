using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies.Switchers
{
    public class NotifyingStrategySwitcher : Switcher<IUserSettingChangedStrategy>
    {

        protected IUserSettingChangedStrategy CurrentStrategy { get; set; }

        public NotifyingStrategySwitcher(IUserSettingChangedStrategy newStrategy)
            : base(newStrategy)
        {
            CurrentStrategy = newStrategy;
        }

        public override void Dispose()
        {
            base.Dispose();
            var strategeyAsDisposable = CurrentStrategy as IDisposable;
            if (strategeyAsDisposable != null)
                strategeyAsDisposable.Dispose();
        }
    }
}
