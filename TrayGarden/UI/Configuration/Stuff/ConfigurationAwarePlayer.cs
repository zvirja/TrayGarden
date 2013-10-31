using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Configuration.Stuff
{
    public class ConfigurationAwarePlayer : IConfigurationAwarePlayer
    {
        protected bool _requiresApplicationReboot;
        public string SettingName { get; protected set; }
        public bool SupportsReset { get; protected set; }
        public bool ReadOnly { get; set; }

        public virtual bool BoolValue { get; set; }
        public virtual int IntValue { get; set; }
        public virtual string StringValue { get; set; }
        public virtual string StringOptionValue { get; set; }
        public virtual object ObjectValue { get; set; }
        public virtual object StringOptions { get; protected set; }
        public virtual string SettingDescription { get; protected set; }
        public virtual bool RequiresApplicationReboot
        {
            get { return _requiresApplicationReboot; }
            set
            {
                _requiresApplicationReboot = value;
                OnRequiresApplicationRebootChanged();
            }
        }

        public event Action ValueChanged;
        public event Action RequiresApplicationRebootChanged;

        protected virtual void OnValueChanged()
        {
            Action handler = ValueChanged;
            if (handler != null) handler();
        }

        protected virtual void OnRequiresApplicationRebootChanged()
        {
            Action handler = RequiresApplicationRebootChanged;
            if (handler != null) handler();
        }

        public virtual void Reset() { }

        public ConfigurationAwarePlayer([NotNull] string settingName, bool supportsReset, bool readOnly)
        {
            Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
            SettingName = settingName;
            SupportsReset = supportsReset;
            ReadOnly = readOnly;
        }

    }
}
