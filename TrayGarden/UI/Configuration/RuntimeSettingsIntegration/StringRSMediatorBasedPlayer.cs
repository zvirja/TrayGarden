using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings.FastPropertyWrapper;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.Configuration.RuntimeSettingsIntegration
{
  public class StringRSMediatorBasedPlayer : ConfigurationAwarePlayer
  {
    protected StringSettingMediator Mediator { get; set; }
    public override string StringValue
    {
      get { return Mediator.Value; }
      set { Mediator.Value = value; }
    }

    public StringRSMediatorBasedPlayer([NotNull] string settingName, StringSettingMediator mediator)
      : base(settingName, true, false)
    {
      Mediator = mediator;
    }

    public override void Reset()
    {
      base.Reset();
      StringValue = Mediator.DefaultValue;
      OnValueChanged();
    }
  }
}
