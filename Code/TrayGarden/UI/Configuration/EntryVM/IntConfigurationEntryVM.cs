using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public class IntConfigurationEntryVM : TypedConfigurationEntryVM<int>
{
  public IntConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<int> realPlayer)
    : base(realPlayer)
  {
  }
}