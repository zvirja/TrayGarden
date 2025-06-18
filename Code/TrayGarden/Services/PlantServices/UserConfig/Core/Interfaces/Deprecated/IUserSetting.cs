using System;

using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;
using TrayGarden.Services.PlantServices.UserConfig.Core.Stuff;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

[Obsolete]
public interface IUserSetting : 
  IIntUserSetting, IBoolUserSetting, IStringUserSetting, IStringOptionUserSetting, IDoubleUserSetting
{
}