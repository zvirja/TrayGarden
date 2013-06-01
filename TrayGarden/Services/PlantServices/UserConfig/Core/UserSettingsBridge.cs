using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Linq;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;
using TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies;
using TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies.Switchers;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
    public class UserSettingsBridge : IUserSettingsBridgeMaster
    {
        protected bool Initialized { get; set; }
        protected Dictionary<string,IUserSetting> UserSettings { get; set; }
        protected IUserSettingChangedStrategy DefaultNotifyingStrategy { get; set; }

        public virtual event UserSettingValuesChanged SettingValuesChanged;

        public UserSettingsBridge()
        {
            Initialized = false;
            DefaultNotifyingStrategy = null;
        }

        public virtual void Initialize([NotNull] IEnumerable<IUserSettingMaster> userSettings,
                                       [NotNull] IUserSettingChangedStrategy defaultNotifyingStrategy)
        {
            Assert.ArgumentNotNull(userSettings, "userSettings");
            Assert.ArgumentNotNull(defaultNotifyingStrategy, "defaultNotifyingStrategy");
            DefaultNotifyingStrategy = defaultNotifyingStrategy;
            var userSettingsDictionary = new Dictionary<string, IUserSetting>();
            foreach (IUserSettingMaster userSetting in userSettings)
            {
                userSettingsDictionary[userSetting.Name] = userSetting;
                SubscribeToSettingChangedEvent(userSetting);
            }
            UserSettings = userSettingsDictionary;
            Initialized = true;
        }

        public virtual IUserSetting GetUserSetting(string name)
        {
            AssertInitialized();
            return UserSettings.ContainsKey(name) ? UserSettings[name] : null;
        }

        public virtual List<IUserSetting> GetUserSettings()
        {
            AssertInitialized();
            return UserSettings.Select(x => x.Value).ToList();
        }


        public virtual void RaiseSettingsChangedEvent(List<IUserSettingChange> changes)
        {
            AssertInitialized();
            OnSettingValuesChanged(changes);
        }

        public virtual void FakeRaiseSettingChange(IUserSetting oldValue, IUserSetting newValue)
        {
            SettingChanged(oldValue,newValue);
        }

        protected virtual void AssertInitialized()
        {
            if(!Initialized)
                throw new NonInitializedException();
        }

        protected virtual void OnSettingValuesChanged(List<IUserSettingChange> changes)
        {
            UserSettingValuesChanged handler = SettingValuesChanged;
            if (handler != null) handler(changes);
        }

        protected virtual void SubscribeToSettingChangedEvent(IUserSettingMaster setting)
        {
            setting.Changed += SettingChanged;
        }

        protected virtual void SettingChanged(IUserSetting stateBefore, IUserSetting stateAfter)
        {
            IUserSettingChangedStrategy currentStrategy = ResolveNotifyingStrategy();
            currentStrategy.NotifySettingChanged(stateBefore,stateAfter,this);
        }

        protected virtual IUserSettingChangedStrategy ResolveNotifyingStrategy()
        {
            return NotifyingStrategySwitcher.CurrentValue ?? DefaultNotifyingStrategy;
        }
    }
}