#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration
{
  public interface IObjectFactory
  {
    #region Public Methods and Operators

    object GetObject();

    object GetPurelyNewObject();

    #endregion
  }
}