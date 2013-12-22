using System;

namespace TrayGarden.RuntimeSettings
{
  public interface ISettingsBox
  {
    event Action OnSaving;
    string this[string settingName] { get; set; }
    string GetString(string settingName, string fallbackValue);
    void SetString(string settingName, string settingValue);
    int GetInt(string settingName, int fallbackValue);
    void SetInt(string settingName, int value);
    double GetDouble(string settingName, double fallbackValue);
    void SetDouble(string settingName, double value);

    bool GetBool(string settingName, bool fallbackValue);
    void SetBool(string settingName, bool value);

    bool TryGetBool(string settingName, out bool value);
    bool TryGetInt(string settingName, out int value);
    bool TryGetDouble(string settingName, out double value);

    ISettingsBox GetSubBox(string boxName);
    void Save();
  }
}