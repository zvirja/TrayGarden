using System;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

namespace TrayGarden.Services.PlantServices.UserConfig.Core;

public class UserSettingBaseChange : EventArgs
{
  public UserSettingBaseChange(IUserSettingBase origin)
  {
    Origin = origin;
  }

  public IUserSettingBase Origin { get; set; }
}