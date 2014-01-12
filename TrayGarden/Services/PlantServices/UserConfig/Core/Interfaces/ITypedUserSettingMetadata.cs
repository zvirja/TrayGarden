using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface ITypedUserSettingMetadata<T> : IUserSettingMetadataBase
  {
    T DefaultValue { get; }
  }
}
