using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific
{
  public class TypedUserSettingChange<T> : UserSettingBaseChange
  {
    #region Constructors and Destructors

    public TypedUserSettingChange(IUserSettingBase origin, T oldValue, T newValue)
      : base(origin)
    {
    }

    #endregion

    #region Public Properties

    public T NewValue { get; protected set; }

    public T OldValue { get; protected set; }

    #endregion
  }
}