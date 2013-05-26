using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class ProvidePlantWithBridge
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantUCPipelineArg args)
        {
            args.Workhorse.StoreUserSettingsBridge(args.Bridge);
        }

       
    }
}
