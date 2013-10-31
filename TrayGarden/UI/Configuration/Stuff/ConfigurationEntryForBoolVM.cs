using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryForBoolVM : ConfigurationEntryVMBase
  {

    public bool BoolValue
    {
      get { return GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().BoolValue; }
      set
      {
        if (value == GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().BoolValue) return;
        GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().BoolValue = value;
        OnPropertyChanged("BoolValue");
      }
    }

    public ConfigurationEntryForBoolVM([NotNull] IConfigurationAwarePlayerWithValues realPlayer)
      : base(realPlayer)
    {
    }

    protected override void OnUnderlyingSettingValueChanged()
    {
      base.OnUnderlyingSettingValueChanged();
      OnPropertyChanged("BoolValue");
    }
  }
}