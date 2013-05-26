using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class ExtractSettingsMetadata
    {
        [UsedImplicitly]
        public void Process(InitPlantUCPipelineArg args)
        {
            IUserSettingsMetadataBuilderMaster metadataBuilder = GetMetadataBuilder();
            if (!args.Workhorse.GetUserSettingsMetadata(metadataBuilder))
            {
                args.Abort();
                return;
            }
            args.SettingsMetadata = metadataBuilder.GetResultingSettingsMetadata();
        }

        protected virtual IUserSettingsMetadataBuilderMaster GetMetadataBuilder()
        {
            return new UserSettingsMetadataBuilder();
        }
    }
}
