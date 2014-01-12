using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.UI.UserSettingPlayers
{
  public class UserSettingStringOptionPlayer : ConfigurationAwarePlayer
  {
    #region Constructors and Destructors

    public UserSettingStringOptionPlayer([NotNull] IStringOptionUserSetting userSetting)
      : base(userSetting.Name, true, false)
    {
      Assert.ArgumentNotNull(userSetting, "UserSetting");
      this.UserSetting = userSetting;
      this.UserSetting.ValueChanged += (before, after) => this.OnValueChanged();
    }

    #endregion

    #region Public Properties

    public override string StringOptionValue
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

    public override object StringOptions
    {
      get
      {
        return this.UserSetting.Metadata.AdditionalParams;
      }
    }

    public IStringOptionUserSetting UserSetting { get; set; }

    #endregion

    #region Public Methods and Operators

    public override void Reset()
    {
      base.Reset();
      this.UserSetting.ResetToDefault();
    }

    #endregion
  }
}