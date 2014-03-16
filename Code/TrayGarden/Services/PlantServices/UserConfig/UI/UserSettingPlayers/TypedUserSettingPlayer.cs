#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.UI.Configuration.EntryVM.Players;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.UI.UserSettingPlayers
{
  public class TypedUserSettingPlayer<TValue> : TypedConfigurationPlayer<TValue>
  {
    #region Constructors and Destructors

    public TypedUserSettingPlayer([NotNull] ITypedUserSetting<TValue> userSetting)
      : base(userSetting.Title, true, false)
    {
      Assert.ArgumentNotNull(userSetting, "userSetting");
      this.UserSetting = userSetting;
      this.UserSetting.ValueChanged += (before, after) => this.OnValueChanged();
    }

    #endregion

    #region Public Properties

    public override string SettingDescription
    {
      get
      {
        return this.UserSetting.Description;
      }
      protected set
      {
      }
    }

    public ITypedUserSetting<TValue> UserSetting { get; set; }

    public override TValue Value
    {
      get
      {
        return this.UserSetting.Value;
      }
      set
      {
        this.UserSetting.Value = value;
      }
    }

    #endregion

    #region Public Methods and Operators

    public override void Reset()
    {
      this.UserSetting.ResetToDefault();
    }

    #endregion
  }
}