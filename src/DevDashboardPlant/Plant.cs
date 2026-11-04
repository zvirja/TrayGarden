using System.Collections.Generic;
using System.Windows;

using TrayGarden.Reception;

namespace DevDashboardPlant;

public class Plant : IPlant, IServicesDelegation
{
  public string Description => "Shows the Dev Dashboard website in a Chromium (WebView2) window that is toggled from its own tray icon.";

  public string HumanSupportingName => "Dev Dashboard";

  public void Initialize()
  {
  }

  public void PostServicesInitialize()
  {
    if (Application.Current != null)
    {
      Application.Current.Exit += (_, _) => DevDashboardController.Instance.Shutdown();
    }
  }

  public List<object> GetServiceDelegates()
  {
    return [DevDashboardIcon.Instance, PlantConfiguration.Instance];
  }
}
