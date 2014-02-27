#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

using TrayGarden.Helpers;

#endregion

namespace TrayGarden.UI.Common.Converters
{
  public class BooleanToVisibilityConverter : IValueConverter
  {
    #region Constructors and Destructors

    public BooleanToVisibilityConverter()
    {
      this.DefaultNonVisibleVisibility = Visibility.Hidden;
    }

    #endregion

    #region Public Properties

    public Visibility DefaultNonVisibleVisibility { get; set; }

    #endregion

    #region Public Methods and Operators

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

    #endregion
  }
}