using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.WindowWithReturn.Integration
{
  public class ExitOnClosePlayer : TypedConfigurationPlayer<bool>
  {
    public ExitOnClosePlayer([NotNull] string settingName, string description)
      : base(settingName, false, false)
    {
      base.SettingDescription = description;
    }

    public override void Reset()
    {

    }

    public override bool Value
    {
      get
      {
        return WindowWithBack.ExitOnClose;
      }
      set
      {
        WindowWithBack.ExitOnClose = value;
      }
    }
  }
}
