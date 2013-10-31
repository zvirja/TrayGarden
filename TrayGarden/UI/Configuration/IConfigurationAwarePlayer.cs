using System;

namespace TrayGarden.UI.Configuration
{
    public interface IConfigurationAwarePlayer
    {
        string SettingName { get; }
        bool SupportsReset { get; }
        bool ReadOnly { get; set; }

        bool BoolValue { get; set; }
        int IntValue { get; set; }
        string StringValue { get; set; }
        string StringOptionValue { get; set; }
        object ObjectValue { get; set; }
        object StringOptions { get;}
        string SettingDescription { get; }
        bool RequiresApplicationReboot { get; }

        event Action ValueChanged;
        event Action RequiresApplicationRebootChanged;

        void Reset();
    }
}