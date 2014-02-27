#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.RuntimeSettings
{
  public interface IRuntimeSettingsManager
  {
    #region Public Properties

    ISettingsBox OtherSettings { get; }

    ISettingsBox SystemSettings { get; }

    #endregion

    #region Public Methods and Operators

    bool SaveNow(bool force);

    #endregion
  }
}