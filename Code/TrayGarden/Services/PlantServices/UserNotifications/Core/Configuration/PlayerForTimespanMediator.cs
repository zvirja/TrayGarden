#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration
{
  public class PlayerForTimespanMediator : TypedConfigurationPlayer<int>
  {
    #region Constructors and Destructors

    public PlayerForTimespanMediator([NotNull] string settingName, TimeSpanSettingMediator mediator)
      : base(settingName, true, false)
    {
      this.Mediator = mediator;
    }

    #endregion

    #region Public Properties

    public override int Value
    {
      get
      {
        return (int)this.Mediator.Value.TotalMilliseconds;
      }
      set
      {
        this.Mediator.Value = TimeSpan.FromMilliseconds(value);
      }
    }

    #endregion

    #region Properties

    protected TimeSpanSettingMediator Mediator { get; set; }

    #endregion

    #region Public Methods and Operators

    public override void Reset()
    {
      this.Mediator.Value = TimeSpan.FromMilliseconds(this.Mediator.DefaultMillisecondsValue);
      this.OnValueChanged();
    }

    #endregion
  }
}