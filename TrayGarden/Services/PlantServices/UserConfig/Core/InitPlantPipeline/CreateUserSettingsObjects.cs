using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class CreateUserSettingsObjects
    {
        public IObjectFactory UserSettingFactory { get; set; }

        [UsedImplicitly]
        public virtual void Process(InitPlantUCPipelineArg args)
        {
            var settingsBox = args.SettingBox;
            var result = args.SettingsMetadata.Select(userSettingMetadataMaster => CreateUserSetting(userSettingMetadataMaster, settingsBox)).ToList();
            args.Settings = result;
        }

        protected virtual IUserSettingMaster GetNewUserSettingObj()
        {
            if (UserSettingFactory != null)
            {
                var newUserSetting = UserSettingFactory.GetPurelyNewObject() as IUserSettingMaster;
                if (newUserSetting != null)
                    return newUserSetting;
            }
            return new UserSetting();
        }

        protected virtual IUserSettingMaster CreateUserSetting(IUserSettingMetadataMaster settingMetadata, ISettingsBox box)
        {
            IUserSettingMaster settingObj = GetNewUserSettingObj();
            settingObj.Initialize(settingMetadata,box);
        }
    }
}
