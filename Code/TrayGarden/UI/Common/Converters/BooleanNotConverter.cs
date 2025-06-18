using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.UI.Common.Converters
{
  public class BooleanNotConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Assert.IsTrue(value is bool, "Passed value should be bool. {0} was passed.".FormatWith(value.GetType().Name));
      var boolValue = (bool)value;
      return !boolValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}