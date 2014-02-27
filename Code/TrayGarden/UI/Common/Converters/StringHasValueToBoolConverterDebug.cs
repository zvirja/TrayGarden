#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using TrayGarden.Helpers;

#endregion

namespace TrayGarden.UI.Common.Converters
{
  public class StringHasValueToBoolConverterDebug : IValueConverter
  {
    #region Public Methods and Operators

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var valueAsStr = value as string;
      return valueAsStr.NotNullNotEmpty();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }

    #endregion
  }
}