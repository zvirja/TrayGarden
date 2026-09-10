using TrayGarden.Services.PlantServices.IsEnabledObserver;

namespace TrayGarden.Reception.Services;

public interface IIsEnabledObserver
{
    void ConsumeIsEnabledInfo(IPlantEnabledInfo plantEnabledInfo);
}