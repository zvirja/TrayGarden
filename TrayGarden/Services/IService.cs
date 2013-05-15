using TrayGarden.Plants;

namespace TrayGarden.Services
{
    public interface IService
    {
        void InitializePlant(IPlant plant);
        void InformInitializeStage();
        void InformDisplayStage();
        void InformClosingStage();
    }
}
