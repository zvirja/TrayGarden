using System;
using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

[Obsolete]
public interface IUserSettingChange
{
  #region Public Properties

  IUserSetting NewUserSetting { get; }

  [CanBeNull]
  IUserSetting OldUserSetting { get; }

  #endregion
}