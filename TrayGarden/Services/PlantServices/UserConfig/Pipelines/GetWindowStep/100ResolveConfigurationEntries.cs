using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.Services.PlantServices.UserConfig.UI.UserSettingPlayers;
using TrayGarden.UI.Configuration.Stuff;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep
{
  [UsedImplicitly]
  public class ResolveConfigurationEntries
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetUCStepPipelineArgs args)
    {
      ConfigurationControlConstructInfo configurationConstructInfo = args.ConfigurationConstructInfo;
      if (configurationConstructInfo.ConfigurationEntries == null)
      {
        configurationConstructInfo.ConfigurationEntries = new List<ConfigurationEntryVMBase>();
      }
      configurationConstructInfo.ConfigurationEntries.AddRange(this.GetSettingVMs(args.UCServicePlantBox));
    }

    #endregion

    #region Methods

    protected virtual ConfigurationEntryVMBase GetConfigurationEntryVMForISetting(IUserSettingBase userSetting)
    {
      if (userSetting is IBoolUserSetting)
      {
        return new ConfigurationEntryForBoolVM(new UserSettingBoolPlayer((IBoolUserSetting)userSetting));
      }
      if (userSetting is IIntUserSetting)
      {
        return new ConfigurationEntryForIntVM(new UserSettingIntPlayer((IIntUserSetting)userSetting));
      }
      if (userSetting is IDoubleUserSetting)
      {
        return new ConfigurationEntryForDoubleVM(new UserSettingDoublePlayer((IDoubleUserSetting)userSetting));
      }
      if (userSetting is IStringUserSetting)
      {
        return new ConfigurationEntryForStringVM(new UserSettingStringPlayer((IStringUserSetting)userSetting));
      }
      if (userSetting is IStringOptionUserSetting)
      {
        return
          new ConfigurationEntryForStringOptionVM(
            new UserSettingStringOptionPlayer((IStringOptionUserSetting)userSetting));
      }
      return null;
    }

    protected virtual IEnumerable<ConfigurationEntryVMBase> GetSettingVMs(UserConfigServicePlantBox ucServicePlantBox)
    {
      Dictionary<string, IUserSettingBase> userSettings = ucServicePlantBox.SettingsSteward.DefinedSettings;
      var result = new List<ConfigurationEntryVMBase>();
      foreach (KeyValuePair<string, IUserSettingBase> userSettingPair in userSettings)
      {
        ConfigurationEntryVMBase resolvedVM = this.GetConfigurationEntryVMForISetting(userSettingPair.Value);
        if (resolvedVM != null)
        {
          result.Add(resolvedVM);
        }
        else
        {
          Log.Warn(
            "Was unable to resolve UserSettingVM for {0} type".FormatWith(userSettingPair.Value.GetType().Name),
            this);
        }
      }
      return result;
    }

    #endregion
  }
}