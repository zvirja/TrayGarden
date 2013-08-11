using System;
using TrayGarden.Plants;

namespace TrayGarden.Services
{
    public interface IService
    {
        string LuggageName { get; }
        string ServiceName { get; }
        string ServiceDescription { get; }
        bool IsEnabled { get; set; }
        bool IsActuallyEnabled { get; }
        bool CanBeDisabled { get; }
        event Action<bool> IsEnabledChanged;

        void InitializePlant(IPlantEx plantEx);
        void InformInitializeStage();
        void InformDisplayStage();
        void InformClosingStage();

        bool IsAvailableForPlant(IPlantEx plantEx);
    }
}
