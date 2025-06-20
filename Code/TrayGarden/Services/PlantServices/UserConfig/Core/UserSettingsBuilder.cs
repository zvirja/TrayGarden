﻿using System.Collections.Generic;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class UserSettingsBuilder : IUserSettingsBuilder
{
  public UserSettingsBuilder(ISettingsBox underlyingBox)
  {
    UnderlyingBox = underlyingBox;
  }

  private ISettingsBox UnderlyingBox { get; set; }

  public virtual IBoolUserSetting BuildBoolSetting(
    string settingName,
    string settingTitle,
    bool defaultValue,
    string settingDescription,
    object additionParams,
    List<IUserSettingBase> parentDependentSetting,
    IUserSettingHallmark hallmark)
  {
    ITypedUserSettingMetadata<bool> settingMetadata = BuildMetadata<bool>(
      settingName,
      settingTitle,
      defaultValue,
      settingDescription,
      additionParams,
      hallmark);
    IBoolUserSetting setting = new BoolUserSetting();
    var masterInstance = (ITypedUserSettingMaster<bool>)setting;
    masterInstance.Initialize(
      settingMetadata,
      new SettingBoxOrientedStorage<bool>(UnderlyingBox.GetBool, UnderlyingBox.SetBool),
      parentDependentSetting);
    return setting;
  }

  public virtual IDoubleUserSetting BuildDoubleSetting(
    string settingName,
    string settingTitle,
    double defaultValue,
    string settingDescription,
    object additionParams,
    List<IUserSettingBase> parentDependentSetting,
    IUserSettingHallmark hallmark)
  {
    ITypedUserSettingMetadata<double> settingMetadata = BuildMetadata<double>(
      settingName,
      settingTitle,
      defaultValue,
      settingDescription,
      additionParams,
      hallmark);
    IDoubleUserSetting setting = new DoubleUserSetting();
    var masterInstance = (ITypedUserSettingMaster<double>)setting;
    masterInstance.Initialize(
      settingMetadata,
      new SettingBoxOrientedStorage<double>(UnderlyingBox.GetDouble, UnderlyingBox.SetDouble),
      parentDependentSetting);
    return setting;
  }

  public virtual IIntUserSetting BuildIntSetting(
    string settingName,
    string settingTitle,
    int defaultValue,
    string settingDescription,
    object additionParams,
    List<IUserSettingBase> parentDependentSetting,
    IUserSettingHallmark hallmark)
  {
    ITypedUserSettingMetadata<int> settingMetadata = BuildMetadata<int>(
      settingName,
      settingTitle,
      defaultValue,
      settingDescription,
      additionParams,
      hallmark);
    var setting = new IntUserSetting();
    var masterInstance = (ITypedUserSettingMaster<int>)setting;
    masterInstance.Initialize(
      settingMetadata,
      new SettingBoxOrientedStorage<int>(UnderlyingBox.GetInt, UnderlyingBox.SetInt),
      parentDependentSetting);
    return setting;
  }

  public virtual IStringOptionUserSetting BuildStringOptionSetting(
    string settingName,
    string settingTitle,
    string defaultValue,
    string settingDescription,
    object additionParams,
    List<IUserSettingBase> parentDependentSetting,
    IUserSettingHallmark hallmark)
  {
    ITypedUserSettingMetadata<string> settingMetadata = BuildMetadata<string>(
      settingName,
      settingTitle,
      defaultValue,
      settingDescription,
      additionParams,
      hallmark);
    var setting = new StringOptionUserSetting();
    var masterInstance = (ITypedUserSettingMaster<string>)setting;
    masterInstance.Initialize(
      settingMetadata,
      new SettingBoxOrientedStorage<string>(UnderlyingBox.GetString, UnderlyingBox.SetString),
      parentDependentSetting);
    return setting;
  }

  public virtual IStringUserSetting BuildStringSetting(
    string settingName,
    string settingTitle,
    string defaultValue,
    string settingDescription,
    object additionParams,
    List<IUserSettingBase> parentDependentSetting,
    IUserSettingHallmark hallmark)
  {
    ITypedUserSettingMetadata<string> settingMetadata = BuildMetadata<string>(
      settingName,
      settingTitle,
      defaultValue,
      settingDescription,
      additionParams,
      hallmark);
    var setting = new StringUserSetting();
    var masterInstance = (ITypedUserSettingMaster<string>)setting;
    masterInstance.Initialize(
      settingMetadata,
      new SettingBoxOrientedStorage<string>(UnderlyingBox.GetString, UnderlyingBox.SetString),
      parentDependentSetting);
    return setting;
  }

  protected virtual ITypedUserSettingMetadata<T> BuildMetadata<T>(
    string name,
    string title,
    T defaultValue,
    string description,
    object additionalParams,
    IUserSettingHallmark hallmark)
  {
    IUserSettingMetadataMaster<T> cleanInstance = GetCleanMetadataInstance<T>();
    cleanInstance.Initialize(name, title, defaultValue, description, additionalParams, hallmark);
    return cleanInstance;
  }

  protected virtual IUserSettingMetadataMaster<T> GetCleanMetadataInstance<T>()
  {
    return new UserSettingMetadata<T>();
  }
}