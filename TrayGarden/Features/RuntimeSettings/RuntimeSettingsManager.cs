using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Features.Contracts;
using TrayGarden.Features.RuntimeSettings.Provider;
using TrayGarden.Helpers;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Features.RuntimeSettings
{
    public class RuntimeSettingsManager : IRequireInitialization
    {
        protected static object _lock = new object();

        protected ISettingsStorage SettingsStorage { get; set; }
        protected IContainer RootContainer { get; set; }

        protected Timer TimerForAutosave { get; set; }
        public int AutoSaveInterval { get; set; }


        public virtual bool SaveNow()
        {
            return SaveSettingsInternal();
        }


        public virtual void Initialize()
        {
            SettingsStorage = HatcherGuide<ISettingsStorage>.Instance;
            SettingsStorage.LoadSettings();
            RootContainer = SettingsStorage.GetRootContainer();

            var autosaveInterval = AutoSaveInterval;
            if (autosaveInterval > 0)
            {
                TimerForAutosave = new Timer(autosaveInterval*1000);
                TimerForAutosave.Elapsed += TimerForAutosave_Elapsed;
                TimerForAutosave.Enabled = true;
            }
        }

        protected virtual void TimerForAutosave_Elapsed(object sender, ElapsedEventArgs e)
        {
            SaveSettingsInternal();
        }

        protected virtual bool SaveSettingsInternal()
        {
            lock (_lock)
            {
                return SettingsStorage.SaveSettings();
            }
        }
    }
}