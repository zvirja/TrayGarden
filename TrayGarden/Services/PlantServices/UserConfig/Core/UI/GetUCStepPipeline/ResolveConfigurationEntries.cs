using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.UI.UserSettingPlayers;
using TrayGarden.UI.Configuration.Stuff;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UI.GetUCStepPipeline
{
    [UsedImplicitly]
    public class ResolveConfigurationEntries
    {
        [UsedImplicitly]
        public virtual void Process(GetUCStepPipelineArgs args)
        {
            ConfigurationControlConstructInfo configurationConstructInfo = args.ConfigurationConstructInfo;
            if(configurationConstructInfo.ConfigurationEntries == null)
                configurationConstructInfo.ConfigurationEntries = new List<ConfigurationEntryVMBase>();
            configurationConstructInfo.ConfigurationEntries.AddRange(GetSettingVMs(args.UCServicePlantBox));
        }

        protected virtual IEnumerable<ConfigurationEntryVMBase> GetSettingVMs(UserConfigServicePlantBox ucServicePlantBox)
        {
            List<IUserSetting> userSettings = ucServicePlantBox.SettingsBridge.GetUserSettings();
            var result = new List<ConfigurationEntryVMBase>();
            foreach (IUserSetting userSetting in userSettings)
            {
                ConfigurationEntryVMBase resolvedVM = GetConfigurationEntryVMForISetting(userSetting);
                if (resolvedVM != null)
                    result.Add(resolvedVM);
                else
                {
                    Log.Warn(
                        "Was unable to resolve UserSettingVM for {0} type".FormatWith(userSetting.StringOptionValue),
                        this);
                }
            }
            return result;
        }

        protected virtual ConfigurationEntryVMBase GetConfigurationEntryVMForISetting(IUserSetting userSetting)
        {
            switch (userSetting.ValueType)
            {
                case UserSettingValueType.Int:
                    return new ConfigurationEntryForIntVM(new UserSettingInt(userSetting)) ;
                case UserSettingValueType.Bool:
                    return new ConfigurationEntryForBoolVM(new UserSettingBool(userSetting)); 
                case UserSettingValueType.String:
                    return new ConfigurationEntryForStringVM(new UserSettingString(userSetting));
                case UserSettingValueType.StringOption:
                    return new ConfigurationEntryForStringOptionVM(new UserSettingStringOption(userSetting));
            }
            return null;
        }
    }
}
