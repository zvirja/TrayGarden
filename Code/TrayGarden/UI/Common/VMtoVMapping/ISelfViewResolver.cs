#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

#endregion

namespace TrayGarden.UI.Common.VMtoVMapping
{
  public interface ISelfViewResolver
  {
    #region Public Methods and Operators

    Control GetViewToPresentMe();

    #endregion
  }
}