using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
  public class UserSettingsBuilder : IUserSettingsBuilder
  {
    #region Constructors and Destructors

    public UserSettingsBuilder(ISettingsBox underlyingBox)
    {
      this.UnderlyingBox = underlyingBox;
    }

    #endregion

    #region Properties

    private ISettingsBox UnderlyingBox { get; set; }

    #endregion

    #region Public Methods and Operators

    public virtual IBoolUserSetting BuildBoolSetting(
      string settingName,
      string settingTitle,
      bool defaultValue,
      string settingDescription,
      object additionParams,
      List<IUserSettingBase> parentDependentSetting,
      IUserSettingHallmark hallmark)
    {
      ITypedUserSettingMetadata<bool> settingMetadata = this.BuildMetadata<bool>(
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
        new SettingBoxOrientedStorage<bool>(this.UnderlyingBox.GetBool, this.UnderlyingBox.SetBool),
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
      ITypedUserSettingMetadata<double> settingMetadata = this.BuildMetadata<double>(
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
        new SettingBoxOrientedStorage<double>(this.UnderlyingBox.GetDouble, this.UnderlyingBox.SetDouble),
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
      ITypedUserSettingMetadata<int> settingMetadata = this.BuildMetadata<int>(
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
        new SettingBoxOrientedStorage<int>(this.UnderlyingBox.GetInt, this.UnderlyingBox.SetInt),
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
      ITypedUserSettingMetadata<string> settingMetadata = this.BuildMetadata<string>(
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
        new SettingBoxOrientedStorage<string>(this.UnderlyingBox.GetString, this.UnderlyingBox.SetString),
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
      ITypedUserSettingMetadata<string> settingMetadata = this.BuildMetadata<string>(
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
        new SettingBoxOrientedStorage<string>(this.UnderlyingBox.GetString, this.UnderlyingBox.SetString),
        parentDependentSetting);
      return setting;
    }

    #endregion

    #region Methods

    protected virtual ITypedUserSettingMetadata<T> BuildMetadata<T>(
      string name,
      string title,
      T defaultValue,
      string description,
      object additionalParams,
      IUserSettingHallmark hallmark)
    {
      IUserSettingMetadataMaster<T> cleanInstance = this.GetCleanMetadataInstance<T>();
      cleanInstance.Initialize(name, title, defaultValue, description, additionalParams, hallmark);
      return cleanInstance;
    }

    protected virtual IUserSettingMetadataMaster<T> GetCleanMetadataInstance<T>()
    {
      return new UserSettingMetadata<T>();
    }

    #endregion
  }
}