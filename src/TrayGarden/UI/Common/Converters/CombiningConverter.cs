using System;
using System.Globalization;
using System.Windows.Data;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Common.Converters;

public class CombiningConverter : IValueConverter
{
  public IValueConverter FirstConverter { get; set; }

  public IValueConverter SecondConverter { get; set; }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    Assert.IsNotNull(FirstConverter, "First conveter cannot be null");
    Assert.IsNotNull(SecondConverter, "Second conveter cannot be null");
    object valueAfterFirstConversion = FirstConverter.Convert(value, targetType, parameter, culture);
    return SecondConverter.Convert(valueAfterFirstConversion, targetType, parameter, culture);
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}