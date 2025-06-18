using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public abstract class TypedConfigurationEntryVM<TEntry> : ConfigurationEntryBaseVM
{
  public TypedConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<TEntry> realPlayer)
    : base(realPlayer)
  {
    this.RealPlayer = realPlayer;
  }

  public new ITypedConfigurationPlayer<TEntry> RealPlayer { get; set; }

  public TEntry Value
  {
    get
    {
      return this.RealPlayer.Value;
    }
    set
    {
      this.RealPlayer.Value = value;
      this.OnPropertyChanged("Value");
    }
  }

  protected override void OnUnderlyingSettingValueChanged()
  {
    base.OnPropertyChanged("Value");
  }
}