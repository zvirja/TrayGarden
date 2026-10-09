using System.Collections.Generic;

using TrayGarden.Reception;

namespace UngoogledChromiumUpdaterPlant;

public class Plant : IPlant, IServicesDelegation
{
  public string Description => "Downloads new Ungoogled Chromium releases in the background and shows a tray icon that installs the update.";

  public string HumanSupportingName => "Ungoogled Chromium updater";

  public void Initialize()
  {
  }

  public void PostServicesInitialize()
  {
    UpdateController.Instance.Start();
  }

  public List<object> GetServiceDelegates()
  {
    return [UpdateController.Instance, PlantConfiguration.Instance];
  }
}
