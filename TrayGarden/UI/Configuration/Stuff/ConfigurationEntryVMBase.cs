using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.Stuff.ExtentedEntry;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryVMBase : INotifyPropertyChanged
  {
    #region Constructors and Destructors

    public ConfigurationEntryVMBase([NotNull] IConfigurationAwarePlayer realPlayer)
    {
      Assert.ArgumentNotNull(realPlayer, "realPlayer");
      this.RealPlayer = realPlayer;
      this.RestoreDefaultValue = new RelayCommand(this.ResetValue, this.RealPlayer.SupportsReset);
      this.RestoreDefaultValueTooltip = "Reset to default";
      this.RealPlayer.ValueChanged += this.OnUnderlyingSettingValueChanged;
      this.RealPlayer.RequiresApplicationRebootChanged += this.RealPlayer_RequiresApplicationRebootChanged;
    }

    #endregion

    #region Public Events

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region Public Properties

    public List<ISettingEntryAction> AdditionalActions
    {
      get
      {
        return this.RealPlayer.AdditionalActions;
      }
    }

    public virtual bool AllowEditing
    {
      get
      {
        return !this.RealPlayer.ReadOnly;
      }
    }

    public virtual bool HideResetButton
    {
      get
      {
        return this.RealPlayer.HideReset;
      }
    }

    public IConfigurationAwarePlayer RealPlayer { get; set; }

    public virtual bool RequiresApplicationReboot
    {
      get
      {
        return this.RealPlayer.RequiresApplicationReboot;
      }
    }

    public ICommand RestoreDefaultValue { get; set; }

    public string RestoreDefaultValueTooltip { get; set; }

    public virtual string SettingDescription
    {
      get
      {
        return this.RealPlayer.SettingDescription;
      }
    }

    public virtual string SettingName
    {
      get
      {
        return this.RealPlayer.SettingName;
      }
    }

    #endregion

    #region Methods

    protected T GetSpecificRealPlayer<T>()
    {
      return (T)this.RealPlayer;
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

    protected virtual void OnUnderlyingSettingValueChanged()
    {
    }

    protected virtual void RealPlayer_RequiresApplicationRebootChanged()
    {
      this.OnPropertyChanged("RequiresApplicationReboot");
    }

    protected virtual void ResetValue(object o)
    {
      if (this.RealPlayer.SupportsReset && this.AllowEditing)
      {
        this.RealPlayer.Reset();
      }
    }

    #endregion
  }
}