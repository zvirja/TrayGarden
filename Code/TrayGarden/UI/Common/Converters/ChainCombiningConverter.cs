using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace TrayGarden.UI.Common.Converters;

public class ChainCombiningConverter : IValueConverter
{
  public ChainCombiningConverter()
  {
    this.ConventerChain = new List<IValueConverter>();
  }

  public List<IValueConverter> ConventerChain { get; set; }

  public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
  {
    object result = value;
    foreach (IValueConverter converter in this.ConventerChain)
    {
      result = converter.Convert(result, targetType, parameter, culture);
    }
    return result;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
  {
    throw new NotSupportedException();
  }
}