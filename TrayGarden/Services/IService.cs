using TrayGarden.Plants;

namespace TrayGarden.Services
{
    public interface IService
    {
        string LuggageName { get; }

        void InitializePlant(IPlantEx plantEx);
        void InformInitializeStage();
        void InformDisplayStage();
        void InformClosingStage();

        bool IsAvailableForPlant(IPlantEx plantEx);
    }
}
