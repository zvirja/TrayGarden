using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class PersonalUserSettingsSteward : IPersonalUserSettingsSteward
{
  public PersonalUserSettingsSteward()
  {
    DefinedSettings = new Dictionary<string, IUserSettingBase>();
  }

  public PersonalUserSettingsSteward([NotNull] IUserSettingsBuilder settingsBuilder)
    : this()
  {
    Assert.ArgumentNotNull(settingsBuilder, "settingsFactory");
    SettingsBuilder = settingsBuilder;
  }

  public Dictionary<string, IUserSettingBase> DefinedSettings { get; set; }

  private IUserSettingsBuilder SettingsBuilder { get; set; }

  public IBoolUserSetting DeclareBoolSetting(
    string name,
    string title,
    bool defaultValue,
    string description = null,
    List<IUserSettingBase> parentDependentSetting = null,
    IUserSettingHallmark hallmark = null)
  {
    IBoolUserSetting userSetting = SettingsBuilder.BuildBoolSetting(
      name,
      title,
      defaultValue,
      description,
      null,
      parentDependentSetting,
      hallmark);
    RegisterSetting(userSetting);
    return userSetting;
  }

  public IDoubleUserSetting DeclareDoubleSetting(
    string name,
    string title,
    double defaultValue,
    string description = null,
    List<IUserSettingBase> parentDependentSetting = null,
    IUserSettingHallmark hallmark = null)
  {
    IDoubleUserSetting userSetting = SettingsBuilder.BuildDoubleSetting(
      name,
      title,
      defaultValue,
      description,
      null,
      parentDependentSetting,
      hallmark);
    RegisterSetting(userSetting);
    return userSetting;
  }

  public IIntUserSetting DeclareIntSetting(
    string name,
    string title,
    int defaultValue,
    string description = null,
    List<IUserSettingBase> parentDependentSetting = null,
    IUserSettingHallmark hallmark = null)
  {
    IIntUserSetting userSetting = SettingsBuilder.BuildIntSetting(
      name,
      title,
      defaultValue,
      description,
      null,
      parentDependentSetting,
      hallmark);
    RegisterSetting(userSetting);
    return userSetting;
  }

  public IStringOptionUserSetting DeclareStringOptionSetting(
    string name,
    string title,
    string defaultValue,
    List<string> possibleOptions,
    string description = null,
    List<IUserSettingBase> parentDependentSetting = null,
    IUserSettingHallmark hallmark = null)
  {
    IStringOptionUserSetting userSetting = SettingsBuilder.BuildStringOptionSetting(
      name,
      title,
      defaultValue,
      description,
      possibleOptions,
      parentDependentSetting,
      hallmark);
    RegisterSetting(userSetting);
    return userSetting;
  }

  public IStringUserSetting DeclareStringSetting(
    string name,
    string title,
    string defaultValue,
    string description = null,
    List<IUserSettingBase> parentDependentSetting = null,
    IUserSettingHallmark hallmark = null)
  {
    IStringUserSetting userSetting = SettingsBuilder.BuildStringSetting(
      name,
      title,
      defaultValue,
      description,
      null,
      parentDependentSetting,
      hallmark);
    RegisterSetting(userSetting);
    return userSetting;
  }

  public List<TSetting> GetAllUserSettingsOfType<TSetting>() where TSetting : IUserSettingBase
  {
    return DefinedSettings.Select(x => x.Value).OfType<TSetting>().ToList();
  }

  public TSetting GetUserSettingOfType<TSetting>(string name) where TSetting : IUserSettingBase
  {
    if (!DefinedSettings.ContainsKey(name))
    {
      return default(TSetting);
    }
    var presentValue = DefinedSettings[name];
    return presentValue is TSetting ? (TSetting)presentValue : default(TSetting);
  }

  private void RegisterSetting(IUserSettingBase newSetting)
  {
    if (DefinedSettings.ContainsKey(newSetting.Name))
    {
      throw new InvalidOperationException("Setting with '{0}' name is already present".FormatWith(newSetting.Name));
    }
    DefinedSettings.Add(newSetting.Name, newSetting);
  }
}