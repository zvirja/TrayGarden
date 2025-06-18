﻿using JetBrains.Annotations;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;

[UsedImplicitly]
public class BindPlantBoxToPlant
{
  [UsedImplicitly]
  public virtual void Process(InitPlantGMArgs args)
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