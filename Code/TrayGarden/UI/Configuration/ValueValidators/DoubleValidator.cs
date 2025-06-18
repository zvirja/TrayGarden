using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TrayGarden.UI.Configuration.ValueValidators;

public class DoubleValidator : ValidationRule
{
  public override ValidationResult Validate(object value, CultureInfo cultureInfo)
  {
    if (value == null)
    {
      return new ValidationResult(false, "Must be a double");
    }
    double parsed;
    if (double.TryParse(value.ToString(), out parsed))
    {
      return new ValidationResult(true, null);
    }
    else
    {
      return new ValidationResult(false, "Must be a double");
    }
  }
}