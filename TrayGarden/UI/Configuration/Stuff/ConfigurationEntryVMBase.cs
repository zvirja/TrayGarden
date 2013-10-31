using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.UI.Common;
using TrayGarden.UI.Common.Commands;

namespace TrayGarden.UI.Configuration.Stuff
{
  public class ConfigurationEntryVMBase : INotifyPropertyChanged
  {
    public IConfigurationAwarePlayer RealPlayer { get; set; }
    public ICommand RestoreDefaultValue { get; set; }
    public string RestoreDefaultValueTooltip { get; set; }

    public string SettingName
    {
      get { return RealPlayer.SettingName; }
    }

    public string SettingDescription
    {
      get { return RealPlayer.SettingDescription; }
    }

    public bool AllowEditing
    {
      get { return !RealPlayer.ReadOnly; }
    }

    public bool RequiresApplicationReboot
    {
      get { return RealPlayer.RequiresApplicationReboot; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public ConfigurationEntryVMBase([NotNull] IConfigurationAwarePlayer realPlayer)
    {
      Assert.ArgumentNotNull(realPlayer, "realPlayer");
      RealPlayer = realPlayer;
      RestoreDefaultValue = new RelayCommand(ResetValue, RealPlayer.SupportsReset);
      RestoreDefaultValueTooltip = "Reset to default";
      RealPlayer.ValueChanged += OnUnderlyingSettingValueChanged;
      RealPlayer.RequiresApplicationRebootChanged += RealPlayer_RequiresApplicationRebootChanged;
    }

    protected virtual void RealPlayer_RequiresApplicationRebootChanged()
    {
      OnPropertyChanged("RequiresApplicationReboot");
    }


    protected virtual void ResetValue(object o)
    {
      if (RealPlayer.SupportsReset && AllowEditing)
        RealPlayer.Reset();
    }

    protected virtual void OnUnderlyingSettingValueChanged()
    {

    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler handler = PropertyChanged;
      if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }

    protected T GetSpecificRealPlayer<T>()
    {
      return (T)RealPlayer;
    }
  }
}
