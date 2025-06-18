using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration
{
  public class PlayerForIntRSMediator : TypedConfigurationPlayer<int>
  {
    public PlayerForIntRSMediator([NotNull] string settingName, IntSettingMediator mediator)
      : base(settingName, true, false)
    {
      this.Mediator = mediator;
    }

    public override int Value
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

    protected IntSettingMediator Mediator { get; set; }

    public override void Reset()
    {
      this.Value = this.Mediator.DefaultValue;
      this.OnValueChanged();
    }
  }
}