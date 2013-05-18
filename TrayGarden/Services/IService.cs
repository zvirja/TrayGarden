using TrayGarden.Plants;

namespace TrayGarden.Services
{
    public interface IService
    {
        string LuggageName { get; }

        void InitializePlant(IPlantInternal plantInternal);
        void InformInitializeStage();
        void InformDisplayStage();
        void InformClosingStage();
    }
}
