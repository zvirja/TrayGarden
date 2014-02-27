#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ClipboardChangerPlant.Configuration
{
  public class Factory
  {
    #region Public Properties

    public static ConfigurationBasedFactory ActualFactory
    {
      get
      {
        return ConfigurationBasedFactory.ActualFactory;
      }
    }

    #endregion
  }
}