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
  public class UserSettingDoublePlayer : ConfigurationAwarePlayer
  {
    #region Constructors and Destructors

    public UserSettingDoublePlayer([NotNull] IDoubleUserSetting userSetting)
      : base(userSetting.Name, true, false)
    {
      Assert.ArgumentNotNull(userSetting, "UserSetting");
      this.UserSetting = userSetting;
      this.UserSetting.ValueChanged += (sender, changeArgs) => this.OnValueChanged();
    }

    #endregion

    #region Public Properties

    public override double DoubleValue
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

    public IDoubleUserSetting UserSetting { get; set; }

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