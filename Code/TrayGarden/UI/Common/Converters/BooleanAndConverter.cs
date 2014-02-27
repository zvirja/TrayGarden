#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

#endregion

namespace TrayGarden.UI.Common.Converters
{
  public class BooleanAndConverter : IMultiValueConverter
  {
    #region Public Methods and Operators

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

    #endregion
  }
}