using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces
{
  public interface IUserSettingStorage<T>
  {
    #region Public Methods and Operators

    T ReadValue(string key, T defaultValue);

    void WriteValue(string key, T value);

    #endregion
  }
}