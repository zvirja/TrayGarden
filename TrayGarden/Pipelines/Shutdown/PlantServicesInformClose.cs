using TrayGarden.Pipelines.Startup;
using TrayGarden.Services.Engine;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Pipelines.Shutdown
{
    public class PlantServicesInformClose
    {
        public void Process(ShutdownArgs args)
        {
            HatcherGuide<IServicesSteward>.Instance.InformClosingStage();
        }
    }
}
