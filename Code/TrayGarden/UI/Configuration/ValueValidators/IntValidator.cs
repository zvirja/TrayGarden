using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TrayGarden.UI.Configuration.ValueValidators
{
  public class IntValidator : ValidationRule
  {
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
      if (value == null)
      {
        return new ValidationResult(false, "Must be an integer");
      }
      int parsed;
      if (int.TryParse(value.ToString(), out parsed))
      {
        return new ValidationResult(true, null);
      }
      else
      {
        return new ValidationResult(false, "Must be an integer");
      }
    }
  }
}