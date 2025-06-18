using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class UserSettingBaseChange : EventArgs
{
  public UserSettingBaseChange(IUserSettingBase origin)
  {
    this.Origin = origin;
  }

  public IUserSettingBase Origin { get; set; }
}