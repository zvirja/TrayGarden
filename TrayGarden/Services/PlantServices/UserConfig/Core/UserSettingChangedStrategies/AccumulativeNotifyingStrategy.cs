using System;
using System.Linq;
using System.Collections.Generic;
using TrayGarden.Helpers.ThreadSwitcher;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UserSettingChangedStrategies
{
    public class AccumulativeNotifyingStrategy : ImpatientNotifyingStrategy, IDisposable
    {
        protected Dictionary<IUserSettingsBridgeMaster, List<IUserSettingChange>> AccumulatedValues { get; set; }

        public AccumulativeNotifyingStrategy()
        {
            AccumulatedValues = new Dictionary<IUserSettingsBridgeMaster, List<IUserSettingChange>>();
        }

        public override void NotifySettingChanged(Interfaces.IUserSetting before, Interfaces.IUserSetting after, Interfaces.IUserSettingsBridgeMaster originator)
        {
            IUserSettingChange change = base.GetChange(before, after);
            AccumulateChange(originator,change);
        }

        public virtual void Dispose()
        {
            BroadcastAccumulatedChanges();
        }

        protected virtual void AccumulateChange(IUserSettingsBridgeMaster originator, IUserSettingChange change)
        {
            List<IUserSettingChange> originatorChanges = AccumulatedValues.ContainsKey(originator)
                                                             ? AccumulatedValues[originator]
                                                             : null;
            if (originatorChanges == null)
            {
                originatorChanges = new List<IUserSettingChange>();
                AccumulatedValues[originator] = originatorChanges;
            }
            originatorChanges.Add(change);
        }

        protected virtual void BroadcastAccumulatedChanges()
        {
            foreach (KeyValuePair<IUserSettingsBridgeMaster, List<IUserSettingChange>> originatorChangesPair in AccumulatedValues)
            {
                IUserSettingsBridgeMaster originator = originatorChangesPair.Key;
                List<IUserSettingChange> relatedChanges = originatorChangesPair.Value;
                relatedChanges = CombineRelatedSettingChanges(relatedChanges);
                originator.RaiseSettingsChangedEvent(relatedChanges);
            }
        }


        protected virtual List<IUserSettingChange> CombineRelatedSettingChanges(
            IEnumerable<IUserSettingChange> collectedChanges)
        {
            var groppedValues = new Dictionary<string, IUserSettingChange>();
            foreach (IUserSettingChange userSettingChange in collectedChanges)
            {
                string settingName = userSettingChange.NewUserSetting.Name;
                if (!groppedValues.ContainsKey(settingName))
                {
                    groppedValues[settingName] = userSettingChange;
                    continue;
                }
                IUserSettingChange storedChange = groppedValues[settingName];
                groppedValues[settingName] = GetChange(storedChange.OldUserSetting, userSettingChange.NewUserSetting);
            }
            return groppedValues.Select(x => x.Value).ToList();
        }
    }
}