using System;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class InitializePlant : IPipelineProcessor<InitializePlantArgs>
{
  [UsedImplicitly]
  public void Process(InitializePlantArgs args)
  {
    IPlant plant = args.IPlantObject;
    try
    {
      plant.Initialize();
    }
    catch (Exception ex)
    {
      Log.For(this).Error(ex, "Unable to initialize plant {PlantType}", plant.GetType());
      args.Abort();
    }
  }
}
