#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Reception;

#endregion

namespace SmallApplicationLauncher
{
  public class Launcher : IPlant, IServicesDelegation
  {
    #region Constructors and Destructors

    public Launcher()
    {
      this.HumanSupportingName = "Small Application Launcher";
      this.Description =
        @"This plant provides an ability to specify path to the folder with small applications and display a list of such applications in the context menu.";
    }

    #endregion

    #region Public Properties

    public string Description { get; private set; }

    public string HumanSupportingName { get; private set; }

    #endregion

    #region Public Methods and Operators

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

    #endregion
  }
}