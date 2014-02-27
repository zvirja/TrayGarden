#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.UI.WindowWithReturn.Integration
{
  public class ExitOnClosePlayer : TypedConfigurationPlayer<bool>
  {
    #region Constructors and Destructors

    public ExitOnClosePlayer([NotNull] string settingName, string description)
      : base(settingName, false, false)
    {
      base.SettingDescription = description;
    }

    #endregion

    #region Public Properties

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

    #endregion

    #region Public Methods and Operators

    public override void Reset()
    {
    }

    #endregion
  }
}