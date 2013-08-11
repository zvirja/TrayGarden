using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
    public class ConfigurationEntryForStringVM : ConfigurationEntryVMBase
    {

        public string StringValue
        {
            get { return RealPlayer.StringValue; }
            set
            {
                if (value == RealPlayer.StringValue) return;
                RealPlayer.StringValue = value;
                OnPropertyChanged("StringValue");
            }
        }

        public ConfigurationEntryForStringVM([NotNull] IConfigurationAwarePlayer realPlayer) : base(realPlayer)
        {
        }

        protected override void OnUnderlyingSettingValueChanged()
        {
            base.OnUnderlyingSettingValueChanged();
            OnPropertyChanged("StringValue");
        }
    }
}