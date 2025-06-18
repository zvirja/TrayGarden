using JetBrains.Annotations;

using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun;

public class AutorunPlayer : TypedConfigurationPlayer<bool>
{
  public AutorunPlayer([NotNull] string settingName, string settingDescription)
    : base(settingName, false, false)
  {
    base.SettingDescription = settingDescription;
  }

  public override bool Value
  {
    get
    {
      return ActualAppProperties.RunAtStartup;
    }
    set
    {
      ActualAppProperties.RunAtStartup = value;
      this.OnValueChanged();
    }
  }

  public override void Reset()
  {
  }
}