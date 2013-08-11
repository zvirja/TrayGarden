using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.LifeCycle;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.Configuration
{
    public class ConfigurationControlVM : IConfigurationControlVM, INotifyPropertyChanged
    {
        protected bool _calculateRebootOption;
        protected bool RebootCommandAllowed { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<ConfigurationEntryVMBase> ConfigurationEntries { get; protected set; }
        public ICommand ResetAll { get; protected set; }
        public ICommand RebootApplication { get; protected set; }

        public string ConfigurationDescription { get; set; }
        public bool CalculateRebootOption
        {
            get { return _calculateRebootOption; }
            set
            {
                if (value.Equals(_calculateRebootOption)) return;
                _calculateRebootOption = value;
                OnPropertyChanged("CalculateRebootOption");
                OnPropertyChanged("RebootRequired");
            }
        }

        public bool RebootRequired
        {
            get
            {
                if (CalculateRebootOption == false)
                    return false;
                return ConfigurationEntries.Any(x => x.RequiresApplicationReboot);
            }
        }


        public ConfigurationControlVM([NotNull] List<ConfigurationEntryVMBase> configurationEntries, bool allowResetAll)
        {
            Assert.ArgumentNotNull(configurationEntries, "configurationEntries");
            ConfigurationEntries = configurationEntries;
            ResetAll = new RelayCommand(ResetAllExecute, allowResetAll);
            RebootApplication = new RelayCommand(RebootAppExecute, CanRebootAppExecute);
            SubscribeToEntriesEvents(ConfigurationEntries);
            RebootCommandAllowed = true;
            CalculateRebootOption = true;
            ConfigurationDescription =
                "here you configure properties for plant. Normally you don't need reboot to apply them.".FormatWith(
                    Environment.NewLine);
        }

        protected virtual bool CanRebootAppExecute(object o)
        {
            return RebootCommandAllowed;
        }

        protected virtual void RebootAppExecute(object o)
        {
            LifecycleObserver.RestartApp(new[] {StringConstants.OpenConfigDialogStartupKey});
        }

        protected virtual void ResetAllExecute(object o)
        {
            foreach (ConfigurationEntryVMBase configurationEntry in ConfigurationEntries)
            {
                if (configurationEntry.RestoreDefaultValue.CanExecute(o))
                    configurationEntry.RestoreDefaultValue.Execute(o);
            }
        }

        protected void SubscribeToEntriesEvents(IEnumerable<ConfigurationEntryVMBase> configurationEntries)
        {
            foreach (ConfigurationEntryVMBase configurationEntry in configurationEntries)
            {
                configurationEntry.PropertyChanged += ConfigurationEntry_PropertyChanged;
            }
        }

        protected virtual void ConfigurationEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("RequiresApplicationReboot", StringComparison.OrdinalIgnoreCase))
                OnPropertyChanged("RebootRequired");
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
