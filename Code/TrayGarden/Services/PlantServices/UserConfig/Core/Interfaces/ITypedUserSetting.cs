using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface ITypedUserSetting<T> : IUserSettingBase
  {
    #region Public Events

    new event EventHandler<TypedUserSettingChange<T>> ValueChanged;

    #endregion

    #region Public Properties

    new ITypedUserSettingMetadata<T> Metadata { get; set; }

    IUserSettingStorage<T> Storage { get; }

    T Value { get; set; }

    #endregion
  }
}