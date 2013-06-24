using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services
{
    public class ServicePlantBoxBase
    {
        public delegate void ServicePlantBoxEnabledChanged(ServicePlantBoxBase sender, bool newValue);

        public ISettingsBox SettingsBox { get; set; }
        public IPlantEx RelatedPlantEx { get; set; }
        public event ServicePlantBoxEnabledChanged IsEnabledChanged;

        public virtual bool IsEnabled
        {
            get { return SettingsBox.GetBool("enabled", true); }
            //TODO FIX BUG. Settings Box may be null if we just set initial IsEnabled value
            set
            {
                SettingsBox.SetBool("enabled", value);
                OnIsEnabledChanged(value);
            }
        }

        protected virtual void OnIsEnabledChanged(bool newValue)
        {
            ServicePlantBoxEnabledChanged handler = IsEnabledChanged;
            if (handler != null) handler(this, newValue);
        }
    }
}
