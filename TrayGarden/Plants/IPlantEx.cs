using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants
{
    public interface IPlantEx
    {
        object Workhorse { get; set; }
        string ID { get; }
        ISettingsBox MySettingsBox { get; }
        bool IsEnabled { get; set; }
        event PlantEnabledChangedEvent EnabledChanged;

        void Initialize(object workhorse, string id, ISettingsBox mySettingsBox);

        bool HasLuggage(string name);
        object GetLuggage(string name);
        T GetLuggage<T>(string name) where T : class;
        void PutLuggage(string name, object luggage);

    }
}