using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class AssignPlantBox
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantUCPipelineArg args)
        {
            UserConfigServicePlantBox userConfigServicePlantBox = CreatePlantBox(args);
            args.RelatedPlant.PutLuggage(args.LuggageName,userConfigServicePlantBox);
        }

        protected virtual UserConfigServicePlantBox CreatePlantBox(InitPlantUCPipelineArg args)
        {
            var plantBox = new UserConfigServicePlantBox();
            plantBox.PlantEx = args.RelatedPlant;
            plantBox.SettingsBox = args.SettingBox;
            plantBox.SettingsBridge = args.Bridge;
            return plantBox;
        }
    }
}
