using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.UI.Common
{
  public interface IDataTemplateSelector
  {
    System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container);
  }
}