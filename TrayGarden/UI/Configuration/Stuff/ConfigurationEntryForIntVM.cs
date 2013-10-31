using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryForIntVM : ConfigurationEntryVMBase
  {

    public int IntValue
    {
      get { return GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().IntValue; }
      set
      {
        if (value == GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().IntValue) return;
        GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().IntValue = value;
        OnPropertyChanged("IntValue");
      }
    }

    public ConfigurationEntryForIntVM([NotNull] IConfigurationAwarePlayerWithValues realPlayer)
      : base(realPlayer)
    {
    }

    protected override void OnUnderlyingSettingValueChanged()
    {
      base.OnUnderlyingSettingValueChanged();
      OnPropertyChanged("IntValue");
    }
  }
}