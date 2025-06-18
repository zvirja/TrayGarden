using TrayGarden.Plants;
using TrayGarden.Reception.Services;

namespace TrayGarden.Services.PlantServices.IsEnabledObserver;

internal class IsEnabledObserverService : PlantServiceBase<IsEnabledObserverPlantBox>
{
    public IsEnabledObserverService() 
        : base("Is Enabled Info", nameof(IsEnabledObserverService))
    {
        ServiceDescription = "Provides plant with run time info if it's enabled";
    }

    public override bool CanBeDisabled => false;

    public override void InitializePlant(IPlantEx plantEx)
    {
        base.InitializePlant(plantEx);

        var isEnabledObserver = plantEx.GetFirstWorkhorseOfType<IIsEnabledObserver>();
        if (isEnabledObserver == null)
        {
            return;
        }

        var plantBox = new IsEnabledObserverPlantBox(plantEx);
        isEnabledObserver.ConsumeIsEnabledInfo(plantBox);
        
        plantEx.PutLuggage(LuggageName, plantBox);
    }

    protected override void PlantOnEnabledChanged(IPlantEx plantEx, bool newValue)
    {
        base.PlantOnEnabledChanged(plantEx, newValue);
        
        plantEx.GetLuggage<IsEnabledObserverPlantBox>(LuggageName)?.NotifyIsEnabledChanged();
    }
}