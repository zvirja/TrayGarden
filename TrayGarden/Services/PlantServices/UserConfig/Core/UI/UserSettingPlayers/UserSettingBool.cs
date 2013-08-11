using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UI.UserSettingPlayers
{
    public class UserSettingBool: ConfigurationAwarePlayer
    {
        public IUserSetting UserSetting { get; set; }

        public override bool BoolValue
        {
            get { return UserSetting.BoolValue; }
            set { UserSetting.BoolValue = value; }
        }

        public UserSettingBool([NotNull] IUserSetting userSetting)
            : base(userSetting.Name, true, false)
        {
            Assert.ArgumentNotNull(userSetting, "UserSetting");
            UserSetting = userSetting;
            UserSetting.Changed += (before, after) => OnValueChanged();
        }

        public override void Reset()
        {
            base.Reset();
            UserSetting.ResetToDefault();
        }
    }
}
