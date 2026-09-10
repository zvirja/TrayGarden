using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

[UsedImplicitly]
public class ValidateAndAssignSIBox : IPipelineProcessor<InitPlantSIArgs>
{
  [UsedImplicitly]
  public void Process(InitPlantSIArgs args)
  {
    if (!IsSIBoxValid(args.SIBox))
    {
      args.Abort();
      return;
    }
    args.SIBox.RelatedPlantEx = args.PlantEx;
    args.PlantEx.PutLuggage(args.LuggageName, args.SIBox);
  }

  private bool IsSIBoxValid(StandaloneIconPlantBox box)
  {
    if (box == null)
    {
      return false;
    }
    if (box.NotifyIcon == null)
    {
      return false;
    }
    return true;
  }
}
