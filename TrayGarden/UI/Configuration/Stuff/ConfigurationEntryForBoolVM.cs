using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
    public class ConfigurationEntryForBoolVM : ConfigurationEntryVMBase
    {

        public bool BoolValue
        {
            get { return RealPlayer.BoolValue; }
            set
            {
                if (value == RealPlayer.BoolValue) return;
                RealPlayer.BoolValue = value;
                OnPropertyChanged("BoolValue");
            }
        }

        public ConfigurationEntryForBoolVM([NotNull] IConfigurationAwarePlayer realPlayer) : base(realPlayer)
        {
        }

        protected override void OnUnderlyingSettingValueChanged()
        {
            base.OnUnderlyingSettingValueChanged();
            OnPropertyChanged("BoolValue");
        }
    }
}