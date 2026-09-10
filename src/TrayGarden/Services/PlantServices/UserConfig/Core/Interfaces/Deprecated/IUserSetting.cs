using System;
using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces;

[Obsolete]
public interface IUserSetting : 
  IIntUserSetting, IBoolUserSetting, IStringUserSetting, IStringOptionUserSetting, IDoubleUserSetting
{
}