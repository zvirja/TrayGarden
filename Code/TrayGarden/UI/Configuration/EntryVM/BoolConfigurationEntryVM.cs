#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM
{
  public class BoolConfigurationEntryVM : TypedConfigurationEntryVM<bool>
  {
    #region Constructors and Destructors

    public BoolConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<bool> realPlayer)
      : base(realPlayer)
    {
    }

    #endregion
  }
}