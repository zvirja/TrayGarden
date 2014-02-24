using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.Configuration;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  [Obsolete]
    public interface IUserSettingsMetadataBuilderMaster : IUserSettingsMetadataBuilder
    {
        List<IUserSettingMetadataBaseMaster> GetResultingSettingsMetadata();
        void Initialize([NotNull] IObjectFactory metadataInstanceFactory);
    }
}