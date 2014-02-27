#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.FleaMarket.IconChanger
{
  public interface INotifyIconChangerClient
  {
    #region Public Methods and Operators

    void SetIcon(Icon newIcon, int msTimeout);

    void SetIcon(Icon newIcon);

    #endregion
  }
}