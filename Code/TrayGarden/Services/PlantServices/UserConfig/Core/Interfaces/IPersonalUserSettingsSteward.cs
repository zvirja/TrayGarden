#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface IPersonalUserSettingsSteward
  {
    #region Public Properties

    Dictionary<string, IUserSettingBase> DefinedSettings { get; set; }

    #endregion

    #region Public Methods and Operators

    IBoolUserSetting DeclareBoolSetting(
      string name,
      string title,
      bool defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null);

    IDoubleUserSetting DeclareDoubleSetting(
      string name,
      string title,
      double defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null);

    IIntUserSetting DeclareIntSetting(
      string name,
      string title,
      int defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null);

    IStringOptionUserSetting DeclareStringOptionSetting(
      string name,
      string title,
      string defaultValue,
      List<string> possibleOptions,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null);

    IStringUserSetting DeclareStringSetting(
      string name,
      string title,
      string defaultValue,
      string description = null,
      List<IUserSettingBase> parentDependentSetting = null,
      IUserSettingHallmark hallmark = null);

    List<TSetting> GetAllUserSettingsOfType<TSetting>() where TSetting : IUserSettingBase;

    TSetting GetUserSettingOfType<TSetting>(string name) where TSetting : IUserSettingBase;

    #endregion
  }
}