using System.Collections.Generic;
using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public class StringOptionConfigurationEntryVM : TypedConfigurationEntryVM<string>
{
  public StringOptionConfigurationEntryVM([NotNull] IStringOptionConfigurationPlayer realPlayer)
    : base(realPlayer)
  {
    RealPlayer = realPlayer;
  }

  public List<string> AllPossibleOptions
  {
    get
    {
      return GetAllPossibleOptions();
    }
  }

  protected new IStringOptionConfigurationPlayer RealPlayer { get; set; }

  protected virtual List<string> GetAllPossibleOptions()
  {
    return RealPlayer.Options;
  }
}