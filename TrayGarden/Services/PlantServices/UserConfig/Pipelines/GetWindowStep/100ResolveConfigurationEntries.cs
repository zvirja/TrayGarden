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
using TrayGarden.UI.Configuration.EntryVM;
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
        configurationConstructInfo.ConfigurationEntries = new List<ConfigurationEntryBaseVM>();
      }
      configurationConstructInfo.ConfigurationEntries.AddRange(this.GetSettingVMs(args.UCServicePlantBox));
    }

    #endregion

    #region Methods

    protected virtual ConfigurationEntryBaseVM GetConfigurationEntryVMForISetting(IUserSettingBase userSetting)
    {
      if (userSetting is IBoolUserSetting)
      {
        return new BoolConfigurationEntryVM(new TypedUserSettingPlayer<bool>((IBoolUserSetting)userSetting));
      }
      if (userSetting is IIntUserSetting)
      {
        return new IntConfigurationEntryVM(new TypedUserSettingPlayer<int>((IIntUserSetting)userSetting));
      }
      if (userSetting is IDoubleUserSetting)
      {
        return new DoubleConfigurationEntryVM(new TypedUserSettingPlayer<double>((IDoubleUserSetting)userSetting));
      }
      if (userSetting is IStringUserSetting)
      {
        return new StringConfigurationEntryVM(new TypedUserSettingPlayer<string>((IStringUserSetting)userSetting));
      }
      if (userSetting is IStringOptionUserSetting)
      {
        return new StringOptionConfigurationEntryVM(new StringOptionUserSettingPlayer((IStringOptionUserSetting)userSetting));
      }
      return null;
    }

    protected virtual IEnumerable<ConfigurationEntryBaseVM> GetSettingVMs(UserConfigServicePlantBox ucServicePlantBox)
    {
      Dictionary<string, IUserSettingBase> userSettings = ucServicePlantBox.SettingsSteward.DefinedSettings;
      var result = new List<ConfigurationEntryBaseVM>();
      foreach (KeyValuePair<string, IUserSettingBase> userSettingPair in userSettings)
      {
        ConfigurationEntryBaseVM resolvedBaseVm = this.GetConfigurationEntryVMForISetting(userSettingPair.Value);
        if (resolvedBaseVm != null)
        {
          result.Add(resolvedBaseVm);
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