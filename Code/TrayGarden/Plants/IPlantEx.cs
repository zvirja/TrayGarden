using System.Collections.Generic;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants;

public interface IPlantEx
{
  event PlantEnabledChangedEvent EnabledChanged;

  string ID { get; }

  bool IsEnabled { get; set; }

  ISettingsBox MySettingsBox { get; }

  IPlant Plant { get; }

  List<object> Workhorses { get; }

  T GetFirstWorkhorseOfType<T>();

  object GetLuggage(string name);

  T GetLuggage<T>(string name) where T : class;

  bool HasLuggage(string name);

  void Initialize(IPlant plant, List<object> workhorses, string id, ISettingsBox mySettingsBox);

  void PutLuggage(string name, object luggage);
}