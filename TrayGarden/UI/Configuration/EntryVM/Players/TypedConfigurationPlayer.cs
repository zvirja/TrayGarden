#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public abstract class TypedConfigurationPlayer<TValue> : ConfigurationPlayerBase, ITypedConfigurationPlayer<TValue>
  {
    #region Constructors and Destructors

    public TypedConfigurationPlayer([NotNull] string settingName, bool supportsReset, bool readOnly)
      : base(settingName, supportsReset, readOnly)
    {
    }

    #endregion

    #region Public Properties

    public abstract TValue Value { get; set; }

    #endregion
  }
}