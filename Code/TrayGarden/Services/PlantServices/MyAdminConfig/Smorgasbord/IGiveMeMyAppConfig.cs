#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.MyAdminConfig.Smorgasbord
{
  /// <summary>
  /// This service provides plant with appropriate App.config file content (for appropriate assembly).
  /// </summary>
  public interface IGiveMeMyAppConfig
  {
    #region Public Methods and Operators

    void StoreModuleConfiguration(System.Configuration.Configuration moduleConfiguration);

    #endregion
  }
}