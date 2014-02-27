#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
  public class PersonalUserSettingsSteward : IPersonalUserSettingsSteward
  {
    #region Constructors and Destructors

    public PersonalUserSettingsSteward()
    {
      this.DefinedSettings = new Dictionary<string, IUserSettingBase>();
    }

    public PersonalUserSettingsSteward([NotNull] IUserSettingsBuilder settingsBuilder)
      : this()
    {
      Assert.ArgumentNotNull(settingsBuilder, "settingsFactory");
      this.SettingsBuilder = settingsBuilder;
    }

    #endregion

    #region Public Properties

    public Dictionary<string, IUserSettingBase> DefinedSettings { get; set; }

    #endregion

    #region Properties

    protected IUserSettingsBuilder SettingsBuilder { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual IBoolUserSetting DeclareBoolSetting(
      string name,
      string title,
      bool defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null)
    {
      IBoolUserSetting userSetting = this.SettingsBuilder.BuildBoolSetting(
        name,
        title,
        defaultValue,
        description,
        null,
        parentDependentSetting,
        hallmark);
      this.RegisterSetting(userSetting);
      return userSetting;
    }

    public virtual IDoubleUserSetting DeclareDoubleSetting(
      string name,
      string title,
      double defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null)
    {
      IDoubleUserSetting userSetting = this.SettingsBuilder.BuildDoubleSetting(
        name,
        title,
        defaultValue,
        description,
        null,
        parentDependentSetting,
        hallmark);
      this.RegisterSetting(userSetting);
      return userSetting;
    }

    public virtual IIntUserSetting DeclareIntSetting(
      string name,
      string title,
      int defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null)
    {
      IIntUserSetting userSetting = this.SettingsBuilder.BuildIntSetting(
        name,
        title,
        defaultValue,
        description,
        null,
        parentDependentSetting,
        hallmark);
      this.RegisterSetting(userSetting);
      return userSetting;
    }

    public virtual IStringOptionUserSetting DeclareStringOptionSetting(
      string name,
      string title,
      string defaultValue,
      List<string> possibleOptions,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null)
    {
      IStringOptionUserSetting userSetting = this.SettingsBuilder.BuildStringOptionSetting(
        name,
        title,
        defaultValue,
        description,
        possibleOptions,
        parentDependentSetting,
        hallmark);
      this.RegisterSetting(userSetting);
      return userSetting;
    }

    public virtual IStringUserSetting DeclareStringSetting(
      string name,
      string title,
      string defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null)
    {
      IStringUserSetting userSetting = this.SettingsBuilder.BuildStringSetting(
        name,
        title,
        defaultValue,
        description,
        null,
        parentDependentSetting,
        hallmark);
      this.RegisterSetting(userSetting);
      return userSetting;
    }

    public virtual List<TSetting> GetAllUserSettingsOfType<TSetting>() where TSetting : IUserSettingBase
    {
      return this.DefinedSettings.Select(x => x.Value).OfType<TSetting>().ToList();
    }

    public virtual TSetting GetUserSettingOfType<TSetting>(string name) where TSetting : IUserSettingBase
    {
      if (!this.DefinedSettings.ContainsKey(name))
      {
        return default(TSetting);
      }
      var presentValue = this.DefinedSettings[name];
      return presentValue is TSetting ? (TSetting)presentValue : default(TSetting);
    }

    #endregion

    #region Methods

    protected virtual void RegisterSetting(IUserSettingBase newSetting)
    {
      if (this.DefinedSettings.ContainsKey(newSetting.Name))
      {
        throw new InvalidOperationException("Setting with '{0}' name is already present".FormatWith(newSetting.Name));
      }
      this.DefinedSettings.Add(newSetting.Name, newSetting);
    }

    #endregion
  }
}