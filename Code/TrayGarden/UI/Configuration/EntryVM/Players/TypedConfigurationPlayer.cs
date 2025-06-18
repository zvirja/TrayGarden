using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public abstract class TypedConfigurationPlayer<TValue> : ConfigurationPlayerBase, ITypedConfigurationPlayer<TValue>
  {
    public TypedConfigurationPlayer([NotNull] string settingName, bool supportsReset, bool readOnly)
      : base(settingName, supportsReset, readOnly)
    {
    }

    public abstract TValue Value { get; set; }
  }
}