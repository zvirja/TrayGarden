using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryForStringVM : ConfigurationEntryVMBase
  {

    public string StringValue
    {
      get { return GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().StringValue; }
      set
      {
        if (value == GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().StringValue) return;
        GetSpecificRealPlayer<IConfigurationAwarePlayerWithValues>().StringValue = value;
        OnPropertyChanged("StringValue");
      }
    }

    public ConfigurationEntryForStringVM([NotNull] IConfigurationAwarePlayerWithValues realPlayer)
      : base(realPlayer)
    {
    }

    protected override void OnUnderlyingSettingValueChanged()
    {
      base.OnUnderlyingSettingValueChanged();
      OnPropertyChanged("StringValue");
    }
  }
}