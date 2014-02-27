#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit
{
  [UsedImplicitly]
  public class CreatePersonalSettingsSteward
  {
    #region Public Properties

    [UsedImplicitly]
    public IUserSettingsBuilder SettingsBuilder { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantUCPipelineArg args)
    {
      IUserSettingsBuilder settingsBuilder = this.GetSettingBuilder(args.SettingBox);
      args.PersonalSettingsSteward = new PersonalUserSettingsSteward(settingsBuilder);
    }

    #endregion

    #region Methods

    protected IUserSettingsBuilder GetSettingBuilder(ISettingsBox settingBox)
    {
      return this.SettingsBuilder ?? new UserSettingsBuilder(settingBox);
    }

    #endregion
  }
}