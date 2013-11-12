using System.Collections.Generic;
using System.Linq;
using TrayGarden.Reception;

namespace SmallApplicationLauncher
{
  public class Launcher : IPlant, IServicesDelegation
  {
    public string HumanSupportingName { get; private set; }
    public string Description { get; private set; }

    public Launcher()
    {
      HumanSupportingName = "Small Application Launcher";
      Description = @"This plant provides an ability to specify path to the folder with small applications and display a list of such applications in the context menu.";
    }

    public void Initialize()
    {
    }

    public void PostServicesInitialize()
    {
    }

    public List<object> GetServiceDelegates()
    {
      var result = new List<object>
      {
        UserConfiguration.Configuration,
        new ContextMenuExtender()
      };
      return result;
    }
  }
}
