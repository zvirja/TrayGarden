using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM
{
  public abstract class ConfigurationEntryBaseVM : INotifyPropertyChanged
  {
    public ConfigurationEntryBaseVM([NotNull] IConfigurationPlayer realPlayer)
    {
      Assert.ArgumentNotNull(realPlayer, "realPlayer");
      this.RealPlayer = realPlayer;
      this.RestoreDefaultValue = new RelayCommand(this.ResetValue, this.RealPlayer.SupportsReset);
      this.RestoreDefaultValueTooltip = "Reset to default";
      this.RealPlayer.ValueChanged += this.OnUnderlyingSettingValueChanged;
      this.RealPlayer.RequiresApplicationRebootChanged += this.RealPlayer_RequiresApplicationRebootChanged;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public List<IConfigurationEntryAction> AdditionalActions
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

    //Be aware that this property is redefined in derived types (to reflect more specific player).
    public IConfigurationPlayer RealPlayer { get; set; }

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

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = this.PropertyChanged;
      if (handler != null)
      {
        handler(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    protected abstract void OnUnderlyingSettingValueChanged();

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
  }
}