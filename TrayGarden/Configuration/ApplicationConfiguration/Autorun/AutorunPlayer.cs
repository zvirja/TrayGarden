#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun
{
  public class AutorunPlayer : TypedConfigurationPlayer<bool>
  {
    #region Constructors and Destructors

    public AutorunPlayer([NotNull] string settingName, string settingDescription)
      : base(settingName, false, false)
    {
      base.SettingDescription = settingDescription;
    }

    #endregion

    #region Public Properties

    public override bool Value
    {
      get
      {
        return ActualAppProperties.RunAtStartup;
      }
      set
      {
        ActualAppProperties.RunAtStartup = value;
        OnValueChanged();
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