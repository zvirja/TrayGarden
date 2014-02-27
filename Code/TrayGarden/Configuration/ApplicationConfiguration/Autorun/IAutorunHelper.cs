#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration.Autorun
{
  public interface IAutorunHelper
  {
    #region Public Properties

    bool IsAddedToAutorun { get; }

    #endregion

    #region Public Methods and Operators

    bool SetNewAutorunValue(bool runAtStartup);

    #endregion
  }
}