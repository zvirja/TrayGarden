using System.Collections.Generic;
using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryForStringOptionVM : ConfigurationEntryVMBase
  {

    public string StringOptionValue
    {
      get { return GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().StringOptionValue; }
      set
      {
        if (value == GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().StringOptionValue) return;
        GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().StringOptionValue = value;
        OnPropertyChanged("StringOptionValue");
      }
    }

    public List<string> AllPossibleOptions
    {
      get { return GetAllPossibleOptions(); }
    }

    public ConfigurationEntryForStringOptionVM([NotNull] IConfigurationAwarePlayerWithValues realPlayer)
      : base(realPlayer)
    {
    }

    protected override void OnUnderlyingSettingValueChanged()
    {
      base.OnUnderlyingSettingValueChanged();
      OnPropertyChanged("StringOptionValue");
    }

    protected virtual List<string> GetAllPossibleOptions()
    {
      var casted = GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().StringOptions as List<string>;
      return casted ?? new List<string>();
    }
  }
}