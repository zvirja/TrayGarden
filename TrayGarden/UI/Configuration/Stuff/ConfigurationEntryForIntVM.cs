using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
    public class ConfigurationEntryForIntVM : ConfigurationEntryVMBase
    {

        public int IntValue
        {
            get { return RealPlayer.IntValue; }
            set
            {
                if (value == RealPlayer.IntValue) return;
                RealPlayer.IntValue = value;
                OnPropertyChanged("IntValue");
            }
        }

        public ConfigurationEntryForIntVM([NotNull] IConfigurationAwarePlayer realPlayer) : base(realPlayer)
        {
        }

        protected override void OnUnderlyingSettingValueChanged()
        {
            base.OnUnderlyingSettingValueChanged();
            OnPropertyChanged("IntValue");
        }
    }
}