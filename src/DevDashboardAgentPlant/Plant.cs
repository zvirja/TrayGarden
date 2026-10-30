using System.Collections.Generic;

using TrayGarden.Reception;

namespace DevDashboardAgentPlant;

public class Plant : IPlant, IServicesDelegation
{
  public string Description => "Hosts the Dev Dashboard agent, which serves git state of the configured folders over HTTPS while the plant is enabled.";

  public string HumanSupportingName => "Dev Dashboard agent";

  public void Initialize()
  {
  }

  public void PostServicesInitialize()
  {
    AgentController.Instance.Start();
  }

  public List<object> GetServiceDelegates()
  {
    return [AgentController.Instance, PlantConfiguration.Instance];
  }
}
