using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration
{
  public class BoolRSMediatorBasedPlayer : ConfigurationAwarePlayer
  {
    protected BoolSettingMediator Mediator { get; set; }
    public override bool BoolValue
    {
      get { return Mediator.Value; }
      set { Mediator.Value = value; }
    }

    public BoolRSMediatorBasedPlayer([NotNull] string settingName, BoolSettingMediator mediator)
      : base(settingName, true, false)
    {
      Mediator = mediator;
    }

    public override void Reset()
    {
      base.Reset();
      BoolValue = Mediator.DefaultValue;
      OnValueChanged();
    }
  }
}
