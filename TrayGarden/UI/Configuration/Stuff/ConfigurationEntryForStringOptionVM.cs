using System.Collections.Generic;
using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.Stuff
{
    public class ConfigurationEntryForStringOptionVM : ConfigurationEntryVMBase
    {

        public string StringOptionValue
        {
            get { return RealPlayer.StringOptionValue; }
            set
            {
                if (value == RealPlayer.StringOptionValue) return;
                RealPlayer.StringOptionValue = value;
                OnPropertyChanged("StringOptionValue");
            }
        }

        public List<string> AllPossibleOptions
        {
            get { return GetAllPossibleOptions(); }
        }

        public ConfigurationEntryForStringOptionVM([NotNull] IConfigurationAwarePlayer realPlayer) : base(realPlayer)
        {
        }

        protected override void OnUnderlyingSettingValueChanged()
        {
            base.OnUnderlyingSettingValueChanged();
            OnPropertyChanged("StringOptionValue");
        }

        protected virtual List<string> GetAllPossibleOptions()
        {
            var casted = RealPlayer.StringOptions as List<string>;
            return casted ?? new List<string>();
        }
    }
}