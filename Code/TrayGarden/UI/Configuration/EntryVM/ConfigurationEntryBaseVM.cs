using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.Configuration.EntryVM.ExtentedEntry;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public abstract class ConfigurationEntryBaseVM : INotifyPropertyChanged
{
  public ConfigurationEntryBaseVM([NotNull] IConfigurationPlayer realPlayer)
  {
    Assert.ArgumentNotNull(realPlayer, "realPlayer");
    RealPlayer = realPlayer;
    RestoreDefaultValue = new RelayCommand(ResetValue, RealPlayer.SupportsReset);
    RestoreDefaultValueTooltip = "Reset to default";
    RealPlayer.ValueChanged += OnUnderlyingSettingValueChanged;
    RealPlayer.RequiresApplicationRebootChanged += RealPlayer_RequiresApplicationRebootChanged;
  }

  public event PropertyChangedEventHandler PropertyChanged;

  public List<IConfigurationEntryAction> AdditionalActions
  {
    get
    {
      return RealPlayer.AdditionalActions;
    }
  }

  public bool AllowEditing
  {
    get
    {
      return !RealPlayer.ReadOnly;
    }
  }

  public virtual bool HideResetButton
  {
    get
    {
      return RealPlayer.HideReset;
    }
  }

  //Be aware that this property is redefined in derived types (to reflect more specific player).
  public IConfigurationPlayer RealPlayer { get; set; }

  public bool RequiresApplicationReboot
  {
    get
    {
      return RealPlayer.RequiresApplicationReboot;
    }
  }

  public ICommand RestoreDefaultValue { get; set; }

  public string RestoreDefaultValueTooltip { get; set; }

  public string SettingDescription
  {
    get
    {
      return RealPlayer.SettingDescription;
    }
  }

  public string SettingName
  {
    get
    {
      return RealPlayer.SettingName;
    }
  }

  [NotifyPropertyChangedInvocator]
  protected void OnPropertyChanged(string propertyName)
  {
    PropertyChangedEventHandler handler = PropertyChanged;
    if (handler != null)
    {
      handler(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  protected abstract void OnUnderlyingSettingValueChanged();

  private void RealPlayer_RequiresApplicationRebootChanged()
  {
    OnPropertyChanged("RequiresApplicationReboot");
  }

  private void ResetValue(object o)
  {
    if (RealPlayer.SupportsReset && AllowEditing)
    {
      RealPlayer.Reset();
    }
  }
}