#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.RuntimeSettings
{
  public enum BulkUpdateState
  {
    Disabled,

    Enabled
  }

  /// <summary>
  /// Normally, runtime setting values are saved each time you assing some value. Context allows to disable per-operation saving and perform saving once, at the end.
  /// </summary>
  public class BulkSettingsUpdate : Switcher<BulkUpdateState>
  {
    #region Constructors and Destructors

    public BulkSettingsUpdate()
      : base(BulkUpdateState.Enabled)
    {
    }

    #endregion

    #region Public Methods and Operators

    public override void Dispose()
    {
      base.Dispose();
      HatcherGuide<IRuntimeSettingsManager>.Instance.SaveNow(false);
    }

    #endregion
  }
}