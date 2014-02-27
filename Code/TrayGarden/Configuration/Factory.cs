#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration
{
  public class Factory
  {
    #region Public Properties

    public static IFactory Instance
    {
      get
      {
        return ModernFactory.ActualInstance;
      }
    }

    #endregion
  }
}