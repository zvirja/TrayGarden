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
        bool GetBool(string settingName, bool fallbackValue);
        void SetBool(string settingName, bool value);
        ISettingsBox GetSubBox(string boxName);
        void Save();
    }
}