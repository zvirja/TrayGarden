using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;

[UsedImplicitly]
public class BindPlantBoxToPlant : IPipelineProcessor<InitPlantGMArgs>
{
  [UsedImplicitly]
  public void Process(InitPlantGMArgs args)
  {
    if (!(args.IsAdvancedMenuExtendingInUse || args.IsMenuExtendingInUse || args.IsNotifyIconChangerInUse))
    {
      args.Abort();
      return;
    }
    GlobalMenuPlantBox globalMenuPlantBox = args.GMBox;
    globalMenuPlantBox.RelatedPlantEx = args.PlantEx;
    globalMenuPlantBox.RelatedPlantEx.PutLuggage(args.LuggageName, globalMenuPlantBox);
  }
}
