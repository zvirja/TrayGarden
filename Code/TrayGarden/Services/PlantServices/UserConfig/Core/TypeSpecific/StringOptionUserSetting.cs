using System;
using System.Collections.Generic;
using System.Linq;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

public class StringOptionUserSetting : TypedUserSetting<string>, IStringOptionUserSetting
{
  public List<string> PossibleOptions
  {
    get
    {
      return this.Metadata.AdditionalParams as List<string> ?? new List<string>();
    }
  }

  public override string Value
  {
    get
    {
      return base.Value;
    }
    set
    {
      if (!this.IsValidStringOptionValue(value))
      {
        throw new InvalidOperationException("'{0}' is not valid value for this setting".FormatWith(value));
      }
      base.Value = value;
    }
  }

  protected virtual bool IsValidStringOptionValue(string value)
  {
    List<string> possibleOptions = this.Metadata.AdditionalParams as List<string> ?? new List<string>();
    return possibleOptions.Any(x => x.Equals(value, StringComparison.OrdinalIgnoreCase));
  }

  protected override string PullValueFromUnderlyingStorage()
  {
    var pulledValue = base.PullValueFromUnderlyingStorage();
    return this.IsValidStringOptionValue(pulledValue) ? pulledValue : this.Metadata.DefaultValue;
  }
}