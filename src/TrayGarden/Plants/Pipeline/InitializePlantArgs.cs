using System.Collections.Generic;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception;
using TrayGarden.RuntimeSettings;

namespace TrayGarden.Plants.Pipeline;

public class InitializePlantArgs : PipelineArgs
{
  public InitializePlantArgs(object plant, ISettingsBox rootSettingsBox)
  {
    PlantObject = plant;
    RootSettingsBox = rootSettingsBox;
  }

  public IPlant IPlantObject { get; set; }

  public string PlantID { get; set; }

  public object PlantObject { get; private set; }

  public ISettingsBox PlantSettingsBox { get; set; }

  public IPlantEx ResolvedPlantEx { get; set; }

  public ISettingsBox RootSettingsBox { get; private set; }

  public List<object> Workhorses { get; set; }
}