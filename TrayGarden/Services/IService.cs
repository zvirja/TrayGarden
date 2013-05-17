using TrayGarden.Plants;

namespace TrayGarden.Services
{
    public interface IService
    {
        string LuggageName { get; }

        void InitializePlant(IPlant plant);
        void InformInitializeStage();
        void InformDisplayStage();
        void InformClosingStage();
    }
}
