using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

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