using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class ExtractSettingsMetadata
    {
        [UsedImplicitly]
        public IObjectFactory SettingsMetadataBuilderFactory { get; set; }

        [UsedImplicitly]
        public IObjectFactory SettingsMetadataEntryFactory { get; set; }


        [UsedImplicitly]
        public virtual void Process(InitPlantUCPipelineArg args)
        {
            IUserSettingsMetadataBuilderMaster metadataBuilder = GetMetadataBuilder();
            metadataBuilder.Initialize(GetMetadataEntryFactory());
            if (!args.Workhorse.GetUserSettingsMetadata(metadataBuilder))
            {
                args.Abort();
                return;
            }
            args.SettingsMetadata = metadataBuilder.GetResultingSettingsMetadata();
        }

        protected virtual IUserSettingsMetadataBuilderMaster GetMetadataBuilder()
        {
            if (SettingsMetadataBuilderFactory != null)
            {
                var newBuilder =
                    SettingsMetadataBuilderFactory.GetPurelyNewObject() as IUserSettingsMetadataBuilderMaster;
                if (newBuilder != null)
                    return newBuilder;
            }
            return new UserSettingsMetadataBuilder();
        }

        protected virtual IObjectFactory GetMetadataEntryFactory()
         {
             if (SettingsMetadataEntryFactory != null)
                 return SettingsMetadataEntryFactory;
            return new ResolverBasedObjectFactory(() => new UserSettingMetadata(),() => new UserSettingMetadata());
         }
    }
}
