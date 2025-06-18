using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Reception;

namespace SmallApplicationLauncher;

public class Launcher : IPlant, IServicesDelegation
{
  public Launcher()
  {
    this.HumanSupportingName = "Small Application Launcher";
    this.Description =
      @"This plant provides an ability to specify path to the folder with small applications and display a list of such applications in the context menu.";
  }

  public string Description { get; private set; }

  public string HumanSupportingName { get; private set; }

  public List<object> GetServiceDelegates()
  {
    var result = new List<object> { UserConfiguration.Configuration, new ContextMenuExtender() };
    return result;
  }

  public void Initialize()
  {
  }

  public void PostServicesInitialize()
  {
  }
}