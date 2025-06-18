using JetBrains.Annotations;

using TrayGarden.Reception.Services;

namespace TrayGarden.Services.PlantServices.RareCommands.Pipelines.PlantInit;

public class CollectRareCommands
{
  [UsedImplicitly]
  public virtual void Process(InitPlantRareCommandsArgs args)
  {
    var workHorse = args.RelatedPlant.GetFirstWorkhorseOfType<IProvidesRareCommands>();
    if (workHorse == null)
    {
      args.Abort();
      return;
    }
    args.CollectedCommands = workHorse.GetRareCommands();
  }
}