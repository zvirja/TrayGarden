#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public abstract class StringOptionConfigurationPlayer : TypedConfigurationPlayer<string>,
                                                          IStringOptionConfigurationPlayer
  {
    #region Constructors and Destructors

    protected StringOptionConfigurationPlayer([NotNull] string settingName, bool supportsReset, bool readOnly)
      : base(settingName, supportsReset, readOnly)
    {
    }

    #endregion

    #region Public Properties

    public abstract List<string> Options { get; }

    #endregion
  }
}