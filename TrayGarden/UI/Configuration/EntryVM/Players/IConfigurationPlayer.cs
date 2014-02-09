#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public interface IConfigurationPlayer
  {
    #region Public Events

    event Action RequiresApplicationRebootChanged;

    event Action ValueChanged;

    #endregion

    #region Public Properties

    List<IConfigurationEntryAction> AdditionalActions { get; }

    bool HideReset { get; }

    bool ReadOnly { get; }

    bool RequiresApplicationReboot { get; }

    string SettingDescription { get; }

    string SettingName { get; }

    bool SupportsReset { get; }

    #endregion

    #region Public Methods and Operators

    void Reset();

    #endregion
  }
}