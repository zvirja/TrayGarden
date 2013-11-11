using System;
using System.Collections.Generic;
using TrayGarden.UI.Configuration.Stuff.ExtentedEntry;

namespace TrayGarden.UI.Configuration
{
  public interface IConfigurationAwarePlayer
  {
    string SettingName { get; }
    bool SupportsReset { get; }
    bool HideReset { get; }
    bool ReadOnly { get; set; }
    string SettingDescription { get; }
    List<ISettingEntryAction> AdditionalActions { get; }
    bool RequiresApplicationReboot { get; }
    event Action ValueChanged;
    event Action RequiresApplicationRebootChanged;
    void Reset();
  }
}