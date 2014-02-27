#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Configuration.ApplicationConfiguration.Autorun;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration
{
  public static class ActualAppProperties
  {
    #region Public Properties

    public static bool RunAtStartup
    {
      get
      {
        return HatcherGuide<IAutorunHelper>.Instance.IsAddedToAutorun;
      }
      set
      {
        HatcherGuide<IAutorunHelper>.Instance.SetNewAutorunValue(value);
      }
    }

    #endregion
  }
}