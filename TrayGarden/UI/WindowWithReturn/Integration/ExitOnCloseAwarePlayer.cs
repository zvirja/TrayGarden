using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.WindowWithReturn.Integration
{
    public class ExitOnCloseAwarePlayer:ConfigurationAwarePlayer
    {
        public override bool BoolValue
        {
            get
            {
                return WindowWithBack.ExitOnClose;
            }
            set
            {
                WindowWithBack.ExitOnClose = value;
            }
        }

        public ExitOnCloseAwarePlayer([NotNull] string settingName, string description) : base(settingName, false, false)
        {
            base.SettingDescription = description;
        }
    }
}
