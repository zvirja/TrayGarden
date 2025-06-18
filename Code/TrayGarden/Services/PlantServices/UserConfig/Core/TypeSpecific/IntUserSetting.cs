using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific;

public class IntUserSetting : TypedUserSetting<int>, IIntUserSetting
{
}