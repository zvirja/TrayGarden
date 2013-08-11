using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun
{
    public class AutorunAwarePlayer : ConfigurationAwarePlayer
    {
        public override bool BoolValue
        {
            get { return ActualAppProperties.RunAtStartup; }
            set { ActualAppProperties.RunAtStartup = value; }
        }


        public AutorunAwarePlayer([NotNull] string settingName, string settingDescription)
            : base(settingName, false, false)
        {
            base.SettingDescription = settingDescription;
        }
    }
}
