using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface ITypedUserSettingMaster<T> : ITypedUserSetting<T>
  {
    void Initialize(ITypedUserSettingMetadata<T> typedMetadata, IUserSettingStorage<T> storage, List<IUserSettingBase> activityCriterias);
  }
}