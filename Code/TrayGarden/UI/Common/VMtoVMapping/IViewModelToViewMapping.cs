#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

#endregion

namespace TrayGarden.UI.Common.VMtoVMapping
{
  public interface IViewModelToViewMapping
  {
    #region Public Properties

    Type AcceptableViewModelType { get; }

    #endregion

    #region Public Methods and Operators

    Control GetControl(object contextVM);

    #endregion
  }
}