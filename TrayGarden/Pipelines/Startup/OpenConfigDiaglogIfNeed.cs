using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Services.Engine;
using TrayGarden.Services.PlantServices.GlobalMenu.Core;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Startup
{
    public class OpenConfigDiaglogIfNeed
    {
        public void Process(StartupArgs args)
        {
            if (args.StartupParams.Any(x => x.Equals(StringConstants.OpenConfigDialogStartupKey,StringComparison.OrdinalIgnoreCase)))
                SilentlyTryToOpenConfigurationWindow();
        }

        protected virtual void SilentlyTryToOpenConfigurationWindow()
        {
            var serviceInstance =
                HatcherGuide<IServicesSteward>.Instance.Services.FirstOrDefault(
                    x => x.GetType().IsAssignableFrom(typeof(GlobalMenuService))) as GlobalMenuService;
            if (serviceInstance == null)
            {
                Log.Warn("Was unable to run startup configuration window", this);
            }
            else
            {
                serviceInstance.ManuallyOpenConfigurationWindow();
            }

        }
    }
}
