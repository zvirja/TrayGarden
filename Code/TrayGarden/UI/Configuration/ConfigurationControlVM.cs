using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.LifeCycle;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.UI.Configuration;

public class ConfigurationControlVM : IConfigurationControlVM, INotifyPropertyChanged
{
  protected bool _calculateRebootOption;

  public ConfigurationControlVM([NotNull] List<ConfigurationEntryBaseVM> configurationEntries, bool allowResetAll)
  {
    Assert.ArgumentNotNull(configurationEntries, "configurationEntries");
    ConfigurationEntries = configurationEntries;
    ResetAll = new RelayCommand(ResetAllExecute, allowResetAll);
    RebootApplication = new RelayCommand(RebootAppExecute, true);
    SubscribeToEntriesEvents(ConfigurationEntries);
    CalculateRebootOption = true;
    ConfigurationDescription =
      "here you configure properties for plant. Normally you don't need reboot to apply them.".FormatWith(Environment.NewLine);
  }

  public event PropertyChangedEventHandler PropertyChanged;

  /// <summary>
  /// Determines whether "Reboot" button should be calculated. If disabled, the strip will not be displayed even if some entries require reboot.
  /// </summary>
  public bool CalculateRebootOption
  {
    get
    {
      return _calculateRebootOption;
    }
    set
    {
      if (value.Equals(_calculateRebootOption))
      {
        return;
      }
      _calculateRebootOption = value;
      OnPropertyChanged("CalculateRebootOption");
      OnPropertyChanged("RebootRequired");
    }
  }

  public string ConfigurationDescription { get; set; }

  public List<ConfigurationEntryBaseVM> ConfigurationEntries { get; protected set; }

  public ICommand RebootApplication { get; protected set; }

  /// <summary>
  /// Used by View. Specifies whether "Reboot app" strip should be displayed.
  /// </summary>
  public bool RebootRequired
  {
    get
    {
      if (CalculateRebootOption == false)
      {
        return false;
      }
      return ConfigurationEntries.Any(x => x.RequiresApplicationReboot);
    }
  }

  public ICommand ResetAll { get; protected set; }

  protected virtual void ConfigurationEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName.Equals("RequiresApplicationReboot", StringComparison.OrdinalIgnoreCase))
    {
      OnPropertyChanged("RebootRequired");
    }
  }

  [NotifyPropertyChangedInvocator]
  protected virtual void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  protected virtual void RebootAppExecute(object o)
  {
    LifecycleObserver.RestartApp(new[] { StringConstants.OpenConfigDialogStartupKey });
  }

  protected virtual void ResetAllExecute(object o)
  {
    foreach (ConfigurationEntryBaseVM configurationEntry in ConfigurationEntries)
    {
      if (configurationEntry.RestoreDefaultValue.CanExecute(o))
      {
        configurationEntry.RestoreDefaultValue.Execute(o);
      }
    }
  }

  protected void SubscribeToEntriesEvents(IEnumerable<ConfigurationEntryBaseVM> configurationEntries)
  {
    foreach (ConfigurationEntryBaseVM configurationEntry in configurationEntries)
    {
      configurationEntry.PropertyChanged += ConfigurationEntry_PropertyChanged;
    }
  }
}