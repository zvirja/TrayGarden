#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.FleaMarket.IconChanger;

#endregion

namespace TrayGarden.Reception.Services
{
  /// <summary>
  /// This service allows to change the main notify icon.
  /// </summary>
  public interface IChangesGlobalIcon
  {
    #region Public Methods and Operators

    void StoreGlobalIconChangingAssignee(INotifyIconChangerClient notifyIconChangerClient);

    #endregion
  }
}