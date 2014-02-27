#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.FleaMarket.IconChanger;

#endregion

namespace TrayGarden.Reception.Services.StandaloneIcon
{
  /// <summary>
  /// This interface allows to modify the notify icon, created by IStandaloneIcon interface.
  /// Pay attention that object may implement IStandaloneIcon to get it work.
  /// </summary>
  public interface INeedToModifyIcon
  {
    #region Public Methods and Operators

    void StoreIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient);

    #endregion
  }
}