﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Smorgasbord;

namespace TrayGarden
{
    public class MockTypeUserConfig : IUserConfiguration
    {
        public virtual bool GetUserSettingsMetadata(IUserSettingsMetadataBuilder metadataBuilder)
        {
            metadataBuilder.DeclareStringSetting("testStr","<empty>");
            metadataBuilder.DeclareIntSetting("testInt",25);
            return true;
        }

        public virtual void StoreUserSettingsBridge(IUserSettingsBridge userSettingsBridge)
        {
            MUserSettingsBridge = userSettingsBridge;
            MUserSettingsBridge.SettingValuesChanged += MUserSettingsBridge_SettingValuesChanged;
        }

        void MUserSettingsBridge_SettingValuesChanged(List<IUserSettingChange> changes)
        {
            int a = 19;
        }

        protected IUserSettingsBridge MUserSettingsBridge { get; set; }
    }
}