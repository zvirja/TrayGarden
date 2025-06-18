using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration.EntryVM.Players;

namespace TrayGarden.UI.Configuration.EntryVM;

public class ActionConfigurationEntry : ConfigurationEntryBaseVM
{
  public ActionConfigurationEntry([NotNull] IActionConfigurationPlayer realPlayer)
    : base(realPlayer)
  {
    Assert.IsNotNull(realPlayer.Action, "Action cannot be null");
    Assert.IsNotNullOrEmpty(realPlayer.ActionTitle, "Action title cannot be null or empty");
    this.RealPlayer = realPlayer;
  }

  public ICommand Action
  {
    get
    {
      return this.RealPlayer.Action;
    }
  }

  public string ActionTitle
  {
    get
    {
      return this.RealPlayer.ActionTitle;
    }
  }

  public override bool HideResetButton
  {
    get
    {
      return true;
    }
  }

  protected new IActionConfigurationPlayer RealPlayer { get; set; }

  protected override void OnUnderlyingSettingValueChanged()
  {
    //here we do nothing. 
  }
}