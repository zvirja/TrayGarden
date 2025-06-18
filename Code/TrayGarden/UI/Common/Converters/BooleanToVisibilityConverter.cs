using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

using TrayGarden.Helpers;

namespace TrayGarden.UI.Common.Converters
{
  public class BooleanToVisibilityConverter : IValueConverter
  {
    public BooleanToVisibilityConverter()
    {
      this.DefaultNonVisibleVisibility = Visibility.Hidden;
    }

    public Visibility DefaultNonVisibleVisibility { get; set; }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is bool))
      {
        return Visibility.Visible;
      }
      var boolean = (bool)value;
      if (boolean)
      {
        return Visibility.Visible;
      }
      var visibilityParam = parameter as string;
      if (visibilityParam.IsNullOrEmpty())
      {
        return this.DefaultNonVisibleVisibility;
      }
      if (visibilityParam.Equals("collapsed", StringComparison.OrdinalIgnoreCase))
      {
        return Visibility.Collapsed;
      }
      return Visibility.Hidden;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }
  }
}