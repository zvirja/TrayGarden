using System;
using System.Globalization;
using System.Windows.Data;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.UI.Common.Converters;

public class BooleanAndConverter : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    if (values.Length == 0)
    {
      return false;
    }
    foreach (object value in values)
    {
      Assert.IsTrue(value is bool, "Passed value should be bool. {0} was passed.".FormatWith(value.GetType().Name));
      var actualValue = (bool)value;
      if (actualValue == false)
      {
        return false;
      }
    }
    return true;
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}