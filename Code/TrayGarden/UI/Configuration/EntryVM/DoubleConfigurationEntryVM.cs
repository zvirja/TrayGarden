using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public class DoubleConfigurationEntryVM : TypedConfigurationEntryVM<double>
{
  public DoubleConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<double> realPlayer)
    : base(realPlayer)
  {
  }
}