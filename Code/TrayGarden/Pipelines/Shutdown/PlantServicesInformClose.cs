using TrayGarden.Pipelines.Engine;
using TrayGarden.Services.Engine;

namespace TrayGarden.Pipelines.Shutdown;

public class PlantServicesInformClose(IServicesSteward servicesSteward) : IPipelineProcessor<ShutdownArgs>
{
  public void Process(ShutdownArgs args)
  {
    servicesSteward.InformClosingStage();
  }
}
