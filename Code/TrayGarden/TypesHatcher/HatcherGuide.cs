#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.TypesHatcher
{
  public static class HatcherGuide<TKeyInterface>
  {
    #region Public Properties

    public static TKeyInterface Instance
    {
      get
      {
        return (TKeyInterface)HatcherManager.Actual.GetObjectByType(typeof(TKeyInterface));
      }
    }

    #endregion

    #region Public Methods and Operators

    public static TKeyInterface CreateNewInstance()
    {
      return (TKeyInterface)HatcherManager.Actual.GetNewObjectByType(typeof(TKeyInterface));
    }

    #endregion
  }
}