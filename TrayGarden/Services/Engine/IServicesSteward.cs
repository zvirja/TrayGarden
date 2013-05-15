using System.Collections.Generic;
using TrayGarden.Plants;

namespace TrayGarden.Services.Engine
{
    public interface IServicesSteward
    {
        List<IService> Services { get; set; }
        void AquaintPlantWithServices(IPlant plant);
        void InformInitializeStage();
        void InformDisplayStage();
        void InformClosingStage();
    }
}