#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.UI.Configuration.EntryVM
{
  public class ActionConfigurationEntry : ConfigurationEntryBaseVM
  {
    #region Constructors and Destructors

    public ActionConfigurationEntry([NotNull] IActionConfigurationPlayer realPlayer)
      : base(realPlayer)
    {
      Assert.IsNotNull(realPlayer.Action, "Action cannot be null");
      Assert.IsNotNullOrEmpty(realPlayer.ActionTitle, "Action title cannot be null or empty");
      this.RealPlayer = realPlayer;
    }

    #endregion


    protected new IActionConfigurationPlayer RealPlayer { get; set; }

    protected override void OnUnderlyingSettingValueChanged()
    {
      //here we do nothing. 
    }

    #region Public Properties

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

    #endregion
  }
}