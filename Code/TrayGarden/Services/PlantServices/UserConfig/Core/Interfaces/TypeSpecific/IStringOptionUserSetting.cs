using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

public interface IStringOptionUserSetting : ITypedUserSetting<string>
{
  List<string> PossibleOptions { get; }
}