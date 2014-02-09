#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration
{
  public class PlayerForDoubleRSMediator : TypedConfigurationPlayer<double>
  {
    #region Constructors and Destructors

    public PlayerForDoubleRSMediator([NotNull] string settingName, DoubleSettingMediator mediator)
      : base(settingName, true, false)
    {
      this.Mediator = mediator;
    }

    #endregion

    #region Public Properties

    public override double Value
    {
      get
      {
        return this.Mediator.Value;
      }
      set
      {
        this.Mediator.Value = value;
      }
    }

    #endregion

    #region Properties

    protected DoubleSettingMediator Mediator { get; set; }

    #endregion

    #region Public Methods and Operators

    public override void Reset()
    {
      this.Value = this.Mediator.DefaultValue;
      this.OnValueChanged();
    }

    #endregion
  }
}