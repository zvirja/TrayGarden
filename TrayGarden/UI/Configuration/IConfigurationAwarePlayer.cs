using System;

namespace TrayGarden.UI.Configuration
{
  public interface IConfigurationAwarePlayer
  {
    string SettingName { get; }
    bool SupportsReset { get; }
    bool ReadOnly { get; set; }
    string SettingDescription { get; }
    bool RequiresApplicationReboot { get; }
    event Action ValueChanged;
    event Action RequiresApplicationRebootChanged;
    void Reset();
  }
}