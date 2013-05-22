using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.Hallway;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants
{
    public interface IPlantEx
    {
        List<object> Workhorses { get; }
        IPlant Plant { get; }
        string ID { get; }
        ISettingsBox MySettingsBox { get; }
        bool IsEnabled { get; set; }
        event PlantEnabledChangedEvent EnabledChanged;

        void Initialize(IPlant plant,List<object> workhorses, string id, ISettingsBox mySettingsBox);

        bool HasLuggage(string name);
        object GetLuggage(string name);
        T GetLuggage<T>(string name) where T : class;
        void PutLuggage(string name, object luggage);

        T GetFirstWorkhorseOfType<T>();

    }
}