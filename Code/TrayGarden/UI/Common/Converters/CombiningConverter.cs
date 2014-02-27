#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.UI.Common.Converters
{
  public class CombiningConverter : IValueConverter
  {
    #region Public Properties

    public IValueConverter FirstConverter { get; set; }

    public IValueConverter SecondConverter { get; set; }

    #endregion

    #region Public Methods and Operators

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      Assert.IsNotNull(this.FirstConverter, "First conveter cannot be null");
      Assert.IsNotNull(this.SecondConverter, "Second conveter cannot be null");
      object valueAfterFirstConversion = this.FirstConverter.Convert(value, targetType, parameter, culture);
      return this.SecondConverter.Convert(valueAfterFirstConversion, targetType, parameter, culture);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }

    #endregion
  }
}