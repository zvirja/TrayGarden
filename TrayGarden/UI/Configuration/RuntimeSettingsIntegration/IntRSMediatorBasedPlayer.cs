using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration
{
  public class IntRSMediatorBasedPlayer : ConfigurationAwarePlayer
  {
    protected IntSettingMediator Mediator { get; set; }
    public override int IntValue
    {
      get { return Mediator.Value; }
      set { Mediator.Value = value; }
    }

    public IntRSMediatorBasedPlayer([NotNull] string settingName, IntSettingMediator mediator)
      : base(settingName, true, false)
    {
      Mediator = mediator;
    }

    public override void Reset()
    {
      base.Reset();
      IntValue = Mediator.DefaultValue;
      OnValueChanged();
    }
  }
}
