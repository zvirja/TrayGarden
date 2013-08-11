using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Services.Engine.UI.Intergration
{
    public class ConfigurationPlayerServiceAware: ConfigurationAwarePlayer
    {
        public IService InfoSource { get; set; }

        public override bool BoolValue
        {
            get
            {
                return InfoSource.IsEnabled;
            }
            set
            {
                InfoSource.IsEnabled = value;
                OnRequiresApplicationRebootChanged();
            }
        }

        public override string SettingDescription
        {
            get
            {
                return InfoSource.ServiceDescription;
            }
            protected set
            {
                base.SettingDescription = value;
            }
        }

        public override bool RequiresApplicationReboot
        {
            get { return InfoSource.IsEnabled != InfoSource.IsActuallyEnabled; }
            set
            {
                base.RequiresApplicationReboot = value;
            }
        }

        public ConfigurationPlayerServiceAware(IService serviceToManage):base(serviceToManage.ServiceName,serviceToManage.CanBeDisabled,!serviceToManage.CanBeDisabled)
        {
            InfoSource = serviceToManage;
            InfoSource.IsEnabledChanged += x => OnValueChanged();
        }

        public override void Reset()
        {
            base.Reset();
            BoolValue = InfoSource.IsActuallyEnabled;
        }

    }
}
