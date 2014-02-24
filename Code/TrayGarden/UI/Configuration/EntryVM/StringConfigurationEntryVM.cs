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
  public class StringConfigurationEntryVM : TypedConfigurationEntryVM<string>
  {
    #region Constructors and Destructors

    public StringConfigurationEntryVM([NotNull] ITypedConfigurationPlayer<string> realPlayer)
      : base(realPlayer)
    {
    }

    #endregion
  }
}