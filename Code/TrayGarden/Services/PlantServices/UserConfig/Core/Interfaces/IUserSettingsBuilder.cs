using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface IUserSettingsBuilder
  {
    IBoolUserSetting BuildBoolSetting(
      string settingName,
      string settingTitle,
      bool defaultValue,
      string settingDescription,
      object additionParams,
      List<IUserSettingBase> parentDependentSetting,
      IUserSettingHallmark hallmark);

    IDoubleUserSetting BuildDoubleSetting(
      string settingName,
      string settingTitle,
      double defaultValue,
      string settingDescription,
      object additionParams,
      List<IUserSettingBase> parentDependentSetting,
      IUserSettingHallmark hallmark);

    IIntUserSetting BuildIntSetting(
      string settingName,
      string settingTitle,
      int defaultValue,
      string settingDescription,
      object additionParams,
      List<IUserSettingBase> parentDependentSetting,
      IUserSettingHallmark hallmark);

    IStringUserSetting BuildStringSetting(
      string settingName,
      string settingTitle,
      string defaultValue,
      string settingDescription,
      object additionParams,
      List<IUserSettingBase> parentDependentSetting,
      IUserSettingHallmark hallmark);

    IStringOptionUserSetting BuildStringOptionSetting(
      string settingName,
      string settingTitle,
      string defaultValue,
      string settingDescription,
      object additionParams,
      List<IUserSettingBase> parentDependentSetting,
      IUserSettingHallmark hallmark);
  }
}