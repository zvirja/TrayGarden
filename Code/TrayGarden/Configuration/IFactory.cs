#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration
{
  public interface IFactory
  {
    #region Public Methods and Operators

    object GetObject(string objectIdentificator);

    T GetObject<T>(string objectIdentificator) where T : class;

    object GetPurelyNewObject(string objectIdentificator);

    T GetPurelyNewObject<T>(string objectIdentificator) where T : class;

    string GetStringSetting(string settingName, string defaultValue);

    #endregion
  }
}