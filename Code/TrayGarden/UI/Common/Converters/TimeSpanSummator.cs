using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.UI.Common.Converters;

public class TimeSpanSummator : IMultiValueConverter
{
  public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
  {
    TimeSpan resultValue = TimeSpan.Zero;
    bool hasValue = false;
    foreach (object timespanValue in values)
    {
      if (!(timespanValue is TimeSpan))
      {
        Log.Debug("TimeSpanSummator. Passed value type {0} isn't an expected TimeSpan".FormatWith(timespanValue.GetType().FullName), this);
        continue;
      }
      var convertedTimeSpan = (TimeSpan)timespanValue;
      hasValue = true;
      resultValue += convertedTimeSpan;
    }
    return hasValue ? resultValue : DependencyProperty.UnsetValue;
  }

  public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
  {
    throw new InvalidOperationException("TimeSpanSummator doesn't support such convertion");
  }
}