using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public abstract class TypedConfigurationEntryVM<TEntry> : ConfigurationEntryBaseVM
{
  public TypedConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<TEntry> realPlayer)
    : base(realPlayer)
  {
    RealPlayer = realPlayer;
  }

  public new ITypedConfigurationPlayer<TEntry> RealPlayer { get; set; }

  public TEntry Value
  {
    get
    {
      return RealPlayer.Value;
    }
    set
    {
      RealPlayer.Value = value;
      OnPropertyChanged("Value");
    }
  }

  protected override void OnUnderlyingSettingValueChanged()
  {
    base.OnPropertyChanged("Value");
  }
}