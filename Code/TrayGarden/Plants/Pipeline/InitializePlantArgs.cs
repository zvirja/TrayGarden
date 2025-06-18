using System.Collections.Generic;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants.Pipeline;

public class InitializePlantArgs : PipelineArgs
{
  public InitializePlantArgs(object plant, ISettingsBox rootSettingsBox)
  {
    this.PlantObject = plant;
    this.RootSettingsBox = rootSettingsBox;
  }

  public IPlant IPlantObject { get; set; }

  public string PlantID { get; set; }

  public object PlantObject { get; protected set; }

  public ISettingsBox PlantSettingsBox { get; set; }

  public IPlantEx ResolvedPlantEx { get; set; }

  public ISettingsBox RootSettingsBox { get; protected set; }

  public List<object> Workhorses { get; set; }
}