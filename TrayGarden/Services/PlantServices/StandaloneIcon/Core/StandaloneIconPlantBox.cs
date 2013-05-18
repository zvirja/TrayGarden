using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core
{
    public class StandaloneIconPlantBox
    {
        public ISettingsBox SettingsBox { get; set; }
        public NotifyIcon NotifyIcon { get; set; }
        public IPlantInternal PlantInternal { get; set; }

        public virtual bool IsEnabled
        {
            get { return SettingsBox.GetBool("enabled", true); }
            set
            {
                SettingsBox.SetBool("enabled", value);
                FixNIVisibility();
            }
        }

        public virtual void FixNIVisibility()
        {
            if (PlantInternal.IsEnabled)
                NotifyIcon.Visible = IsEnabled;
            else
                NotifyIcon.Visible = false;
        }

    }
}
