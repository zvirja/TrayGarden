using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Smorgasbord
{
    public interface IUserConfiguration
    {
        bool GetUserSettingsMetadata(IUserSettingsMetadataBuilder metadataBuilder);
        void StoreUserSettingsBridge(IUserSettingsBridge userSettingsBridge);
    }
}
