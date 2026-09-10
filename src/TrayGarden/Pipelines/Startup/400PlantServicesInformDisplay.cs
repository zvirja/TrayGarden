using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.Engine;

namespace TrayGarden.Pipelines.Startup;

public class PlantServicesInformDisplay(IServicesSteward servicesSteward) : IPipelineProcessor<StartupArgs>
{
  [UsedImplicitly]
  public void Process(StartupArgs args)
  {
    servicesSteward.InformDisplayStage();
  }
}
