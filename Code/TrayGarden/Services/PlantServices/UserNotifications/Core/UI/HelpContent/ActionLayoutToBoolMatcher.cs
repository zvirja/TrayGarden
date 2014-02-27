#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.UI.HelpContent
{
  public class ActionLayoutToBoolMatcher : IValueConverter
  {
    #region Public Properties

    public ImageTextOrder ValueToMatch { get; set; }

    #endregion

    #region Public Methods and Operators

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      if (!(value is ImageTextOrder))
      {
        return false;
      }
      var converted = (ImageTextOrder)value;
      return this.ValueToMatch == converted;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      throw new NotSupportedException();
    }

    #endregion
  }
}