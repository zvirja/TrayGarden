using System;
using System.Collections.Generic;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

namespace TrayGarden.UI.Configuration.EntryVM.Players;

public interface IConfigurationPlayer
{
  event Action RequiresApplicationRebootChanged;

  event Action ValueChanged;

  List<IConfigurationEntryAction> AdditionalActions { get; }

  bool HideReset { get; }

  bool ReadOnly { get; }

  bool RequiresApplicationReboot { get; }

  string SettingDescription { get; }

  string SettingName { get; }

  bool SupportsReset { get; }

  void Reset();
}