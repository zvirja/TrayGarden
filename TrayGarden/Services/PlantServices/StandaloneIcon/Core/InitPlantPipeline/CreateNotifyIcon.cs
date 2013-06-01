using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class CreateNotifyIcon
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantSIArgs args)
        {
            StandaloneIconPlantBox siBox = args.SIBox;
            ResolveIAdvanced(args);
            if (siBox.NotifyIcon != null)
                return;
            ResolveISimple(args);
            if(siBox.NotifyIcon == null)
                args.Abort();
        }

        protected virtual void ResolveISimple(InitPlantSIArgs args)
        {
            var asSimple = args.PlantEx.GetFirstWorkhorseOfType<IStandaloneIcon>();
            if (asSimple == null)
                return;
            string niTitle;
            Icon niIcon;
            MouseEventHandler niClickHandler;
            if (!asSimple.GetIconInfo(out niTitle, out niIcon, out niClickHandler))
            {
                Log.Warn("Plant inherints StandaloneIcon contract, but wouldn't like return notify icon",this);
                return;
            }
            if (niTitle.IsNullOrEmpty() || niIcon == null || niClickHandler == null)
            {
                Log.Warn("Plant inherints StandaloneIcon contract, but return wrong data", this);
                return;
            }
            var notifyIcon = new NotifyIcon();
            notifyIcon.Visible = false;
            notifyIcon.Text = niTitle;
            notifyIcon.Icon = niIcon;
            notifyIcon.MouseClick += niClickHandler;
            args.SIBox.NotifyIcon = notifyIcon;
        }

        protected virtual void ResolveIAdvanced(InitPlantSIArgs args)
        {
            var asAdvanced = args.PlantEx.GetFirstWorkhorseOfType<IAdvancedStandaloneIcon>();
            if (asAdvanced != null)
            {
                args.SIBox.NotifyIcon = asAdvanced.GetNotifyIcon();
            }
        }
    }
}
