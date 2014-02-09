﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.UI.Configuration.EntryVM.Players
{
  public class ActionConfigurationPlayer : ConfigurationPlayerBase, IActionConfigurationPlayer
  {
    public ActionConfigurationPlayer([NotNull] string settingName,
                                     [NotNull] string title,
                                     [NotNull] ICommand action, bool readOnly = false)
      : base(settingName, false, readOnly)
    {
      Assert.ArgumentNotNullOrEmpty(settingName, "settingName");
      Assert.ArgumentNotNull(title, "title");
      Assert.ArgumentNotNull(action, "action");
      this.ActionTitle = title;
      this.Action = action;
    }

    public override void Reset()
    {

    }

    public ICommand Action { get; protected set; }

    public string ActionTitle { get; protected set; }
  }
}