﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM
{
  public class StringOptionConfigurationEntryVM : TypedConfigurationEntryVM<string>
  {
    public StringOptionConfigurationEntryVM([NotNull] IStringOptionConfigurationPlayer realPlayer)
      : base(realPlayer)
    {
      this.RealPlayer = realPlayer;
    }

    public List<string> AllPossibleOptions
    {
      get
      {
        return this.GetAllPossibleOptions();
      }
    }

    protected new IStringOptionConfigurationPlayer RealPlayer { get; set; }

    protected virtual List<string> GetAllPossibleOptions()
    {
      return this.RealPlayer.Options;
    }
  }
}