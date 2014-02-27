#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core
{
  public class UserSettingBaseChange : EventArgs
  {
    #region Constructors and Destructors

    public UserSettingBaseChange(IUserSettingBase origin)
    {
      this.Origin = origin;
    }

    #endregion

    #region Public Properties

    public IUserSettingBase Origin { get; set; }

    #endregion
  }
}