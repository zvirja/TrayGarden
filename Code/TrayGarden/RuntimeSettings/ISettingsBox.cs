using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.RuntimeSettings;

public interface ISettingsBox
{
  event Action OnSaving;

  string this[string settingName] { get; set; }

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
}