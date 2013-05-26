using System.Collections.Generic;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
    public interface IUserSettingsMetadataBuilderMaster : IUserSettingsMetadataBuilder
    {
        List<IUserSettingMetadataMaster> GetResultingSettingsMetadata();
    }
}