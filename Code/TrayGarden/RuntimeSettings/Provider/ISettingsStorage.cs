#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.RuntimeSettings.Provider
{
  public interface ISettingsStorage
  {
    #region Public Methods and Operators

    IContainer GetRootContainer();

    void LoadSettings();

    bool SaveSettings();

    #endregion
  }
}