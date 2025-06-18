using JetBrains.Annotations;

using TrayGarden.Services.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Startup;

public class PlantServicesInformInit
{
  [UsedImplicitly]
  public void Process(StartupArgs args)
  {
    HatcherGuide<IServicesSteward>.Instance.InformInitializeStage();
  }
}