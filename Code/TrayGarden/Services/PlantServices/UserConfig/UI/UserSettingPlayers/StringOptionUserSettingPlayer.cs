#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.UI.UserSettingPlayers
{
  public class StringOptionUserSettingPlayer : TypedUserSettingPlayer<string>, IStringOptionConfigurationPlayer
  {
    #region Constructors and Destructors

    public StringOptionUserSettingPlayer([NotNull] IStringOptionUserSetting userSetting)
      : base(userSetting)
    {
      this.UserSetting = userSetting;
    }

    #endregion

    #region Public Properties

    public List<string> Options
    {
      get
      {
        return this.UserSetting.PossibleOptions;
      }
    }

    public new IStringOptionUserSetting UserSetting { get; set; }

    #endregion
  }
}