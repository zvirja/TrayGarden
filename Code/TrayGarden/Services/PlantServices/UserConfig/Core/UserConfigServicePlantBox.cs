using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
  public class UserConfigServicePlantBox : ServicePlantBoxBase
  {
    #region Public Properties

    public IPersonalUserSettingsSteward SettingsSteward { get; set; }

    #endregion
  }
}