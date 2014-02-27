#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.UI.Common
{
  public interface IDataTemplateSelector
  {
    #region Public Methods and Operators

    System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container);

    #endregion
  }
}