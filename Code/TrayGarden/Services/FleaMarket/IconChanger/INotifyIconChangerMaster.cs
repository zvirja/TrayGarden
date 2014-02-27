#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Services.FleaMarket.IconChanger
{
  public interface INotifyIconChangerMaster : INotifyIconChangerClient
  {
    #region Public Properties

    int DefaultDelayMsec { get; set; }

    bool IsEnabled { get; set; }

    #endregion

    #region Public Methods and Operators

    void Initialize([NotNull] NotifyIcon operableNIcon);

    #endregion
  }
}