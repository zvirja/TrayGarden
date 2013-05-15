using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class CreateNotifyIcon
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantSIArgs args)
        {
            StandaloneIconPlantBox siBox = args.SIBox;
            var asAdvanced = args.Plant.Workhorse as IAdvancedStandaloneIcon;
            if (asAdvanced != null)
            {
                siBox.NotifyIcon = asAdvanced.GetNotifyIcon();
            }
            if (siBox.NotifyIcon != null)
                return;
            var asSimple = args.Plant.Workhorse as IStandaloneIcon;
            if (asSimple == null)
            {
                args.Abort();
                return;
            }
            siBox.NotifyIcon = new NotifyIcon();

        }
    }
}
