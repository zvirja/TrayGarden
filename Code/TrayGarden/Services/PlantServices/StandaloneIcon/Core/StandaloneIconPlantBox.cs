using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrayGarden.Plants;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core
{
    public class StandaloneIconPlantBox : ServicePlantBoxBase
    {
        public NotifyIcon NotifyIcon { get; set; }

        public StandaloneIconPlantBox()
        {
            base.IsEnabledChanged += StandaloneIconPlantBox_IsEnabledChanged;
        }

        protected virtual void StandaloneIconPlantBox_IsEnabledChanged(ServicePlantBoxBase sender, bool newvalue)
        {
            FixNIVisibility();
        }


        public virtual void FixNIVisibility()
        {
            if (RelatedPlantEx.IsEnabled)
                NotifyIcon.Visible = IsEnabled;
            else
                NotifyIcon.Visible = false;
        }

    }
}
