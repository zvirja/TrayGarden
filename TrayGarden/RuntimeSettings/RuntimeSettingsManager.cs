using System;
using System.Timers;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings.Provider;

namespace TrayGarden.RuntimeSettings
{
    [UsedImplicitly]
    public class RuntimeSettingsManager : IRuntimeSettingsManager
    {
        protected static readonly object _lock = new object();

        protected ISettingsStorage SettingsStorage { get; set; }
        protected IContainer RootContainer { get; set; }
        protected ISettingsBox RootBox { get; set; }

        protected Timer TimerForAutosave { get; set; }
        public int AutoSaveInterval { get; set; }
        
        public virtual ISettingsBox SystemSettings
        {
            get { return RootBox.GetSubBox("system"); }
        }

        public virtual ISettingsBox OtherSettings
        {
            get { return RootBox.GetSubBox("other"); }
        }


        public virtual bool SaveNow()
        {
            return SaveSettingsInternal();
        }

        [UsedImplicitly]
        public virtual void Initialize(ISettingsStorage settingsStorage)
        {
            if (settingsStorage == null)
                throw new ArgumentNullException("settingsStorage");
            SettingsStorage = settingsStorage;
            SettingsStorage.LoadSettings();
            RootContainer = SettingsStorage.GetRootContainer();
            RootBox = GetRootBox(RootContainer);
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
            if (BulkSettingsUpdate.CurrentValue == BulkUpdateState.Enabled)
                return true;
            lock (_lock)
            {
                return SettingsStorage.SaveSettings();
            }
        }

        protected virtual ISettingsBox GetRootBox(IContainer container)
        {
            var rootBox = new ContainerBasedSettingsBox();
            rootBox.Initialize(container);
            rootBox.OnSaving += RootBoxSave;
            return rootBox;
        }

        protected virtual void RootBoxSave()
        {
            SaveSettingsInternal();
        }
    }
}