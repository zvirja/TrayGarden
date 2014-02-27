#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface ITypedUserSettingMetadata<T> : IUserSettingMetadataBase
  {
    #region Public Properties

    T DefaultValue { get; }

    #endregion
  }
}