using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public class BoolConfigurationEntryVM : TypedConfigurationEntryVM<bool>
{
  public BoolConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<bool> realPlayer)
    : base(realPlayer)
  {
  }
}