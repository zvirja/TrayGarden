#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.RuntimeSettings
{
  public interface ISettingsBox
  {
    #region Public Events

    event Action OnSaving;

    #endregion

    #region Public Indexers

    string this[string settingName] { get; set; }

    #endregion

    #region Public Methods and Operators

    bool GetBool(string settingName, bool fallbackValue);

    double GetDouble(string settingName, double fallbackValue);

    int GetInt(string settingName, int fallbackValue);

    string GetString(string settingName, string fallbackValue);

    ISettingsBox GetSubBox(string boxName);

    void Save();

    void SetBool(string settingName, bool value);

    void SetDouble(string settingName, double value);

    void SetInt(string settingName, int value);

    void SetString(string settingName, string settingValue);

    bool TryGetBool(string settingName, out bool value);

    bool TryGetDouble(string settingName, out double value);

    bool TryGetInt(string settingName, out int value);

    #endregion
  }
}