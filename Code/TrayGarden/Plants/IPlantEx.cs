#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

#endregion

namespace TrayGarden.Plants
{
  public interface IPlantEx
  {
    #region Public Events

    event PlantEnabledChangedEvent EnabledChanged;

    #endregion

    #region Public Properties

    string ID { get; }

    bool IsEnabled { get; set; }

    ISettingsBox MySettingsBox { get; }

    IPlant Plant { get; }

    List<object> Workhorses { get; }

    #endregion

    #region Public Methods and Operators

    T GetFirstWorkhorseOfType<T>();

    object GetLuggage(string name);

    T GetLuggage<T>(string name) where T : class;

    bool HasLuggage(string name);

    void Initialize(IPlant plant, List<object> workhorses, string id, ISettingsBox mySettingsBox);

    void PutLuggage(string name, object luggage);

    #endregion
  }
}