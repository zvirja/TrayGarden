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
    public class UserSettingStringOption: ConfigurationAwarePlayer
    {
        public IUserSetting UserSetting { get; set; }

        public override string StringOptionValue
        {
            get { return UserSetting.StringOptionValue; }
            set { UserSetting.StringOptionValue = value; }
        }

        public override object ServiceOptions
        {
            get { return UserSetting.Metadata.AdditionalParams; }
        }

        public UserSettingStringOption([NotNull] IUserSetting userSetting)
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
