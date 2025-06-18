using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public class StringConfigurationEntryVM : TypedConfigurationEntryVM<string>
{
  public StringConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<string> realPlayer)
    : base(realPlayer)
  {
  }
}