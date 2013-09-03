using JetBrains.Annotations;
using TrayGarden.RuntimeSettings;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class BindPlantBoxToPlant
  {
    [UsedImplicitly]
    public virtual void Process(InitPlantGMArgs args)
    {
      if (!args.IsPlantInUse)
      {
        args.Abort();
        return;
      }
      GlobalMenuPlantBox globalMenuPlantBox = args.GMBox;
      globalMenuPlantBox.RelatedPlantEx = args.PlantEx;
      globalMenuPlantBox.GlobalNotifyIconChanger = args.GlobalNotifyIconChanger;
      globalMenuPlantBox.RelatedPlantEx.PutLuggage(args.LuggageName, globalMenuPlantBox);
    }
  }
}
