using System.Collections.Generic;
using TrayGarden.Reception;

namespace CluedInAuthTokenPlant;

public class Plant: IPlant, IServicesDelegation
{
    public string Description => "Service to retrieve CluedIn Auth token";
    
    public string HumanSupportingName => "CluedIn Auth Service";
    
    public void Initialize()
    {
    }

    public void PostServicesInitialize()
    {
    }

    public List<object> GetServiceDelegates()
    {
        return [GlobalMenuHandler.Instance, PlantConfiguration.Instance];
    }
}