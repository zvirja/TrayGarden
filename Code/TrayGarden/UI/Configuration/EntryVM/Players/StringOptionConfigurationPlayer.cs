﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace TrayGarden.UI.Configuration.EntryVM.Players;

public abstract class StringOptionConfigurationPlayer : TypedConfigurationPlayer<string>, IStringOptionConfigurationPlayer
{
  protected StringOptionConfigurationPlayer([NotNull] string settingName, bool supportsReset, bool readOnly)
    : base(settingName, supportsReset, readOnly)
  {
  }

  public abstract List<string> Options { get; }
}