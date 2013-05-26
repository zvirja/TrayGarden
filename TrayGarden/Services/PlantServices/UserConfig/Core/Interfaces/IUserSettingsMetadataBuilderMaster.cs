using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.Configuration;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingsMetadataBuilderMaster : IUserSettingsMetadataBuilder
    {
        List<IUserSettingMetadataMaster> GetResultingSettingsMetadata();
        void Initialize([NotNull] IObjectFactory metadataInstanceFactory);
    }
}