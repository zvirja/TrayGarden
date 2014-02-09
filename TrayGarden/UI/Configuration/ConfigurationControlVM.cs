#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.LifeCycle;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.EntryVM;

#endregion

namespace TrayGarden.UI.Configuration
{
  public class ConfigurationControlVM : IConfigurationControlVM, INotifyPropertyChanged
  {
    #region Fields

    protected bool _calculateRebootOption;

    #endregion

    #region Constructors and Destructors

    public ConfigurationControlVM([NotNull] List<ConfigurationEntryBaseVM> configurationEntries, bool allowResetAll)
    {
      Assert.ArgumentNotNull(configurationEntries, "configurationEntries");
      this.ConfigurationEntries = configurationEntries;
      this.ResetAll = new RelayCommand(this.ResetAllExecute, allowResetAll);
      this.RebootApplication = new RelayCommand(this.RebootAppExecute, true);
      this.SubscribeToEntriesEvents(this.ConfigurationEntries);
      this.CalculateRebootOption = true;
      this.ConfigurationDescription =
        "here you configure properties for plant. Normally you don't need reboot to apply them.".FormatWith(
          Environment.NewLine);
    }

    #endregion

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Public Properties

    /// <summary>
    /// Determines whether "Reboot" button should be calculated. If disabled, the strip will not be displayed even if some entries require reboot.
    /// </summary>
    public bool CalculateRebootOption
    {
      get
      {
        return this._calculateRebootOption;
      }
      set
      {
        if (value.Equals(this._calculateRebootOption))
        {
          return;
        }
        this._calculateRebootOption = value;
        this.OnPropertyChanged("CalculateRebootOption");
        this.OnPropertyChanged("RebootRequired");
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
        if (this.CalculateRebootOption == false)
        {
          return false;
        }
        return this.ConfigurationEntries.Any(x => x.RequiresApplicationReboot);
      }
    }

    public ICommand ResetAll { get; protected set; }

    #endregion

    #region Methods

    protected virtual void ConfigurationEntry_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName.Equals("RequiresApplicationReboot", StringComparison.OrdinalIgnoreCase))
      {
        this.OnPropertyChanged("RebootRequired");
      }
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
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
      foreach (ConfigurationEntryBaseVM configurationEntry in this.ConfigurationEntries)
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
        configurationEntry.PropertyChanged += this.ConfigurationEntry_PropertyChanged;
      }
    }

    #endregion
  }
}