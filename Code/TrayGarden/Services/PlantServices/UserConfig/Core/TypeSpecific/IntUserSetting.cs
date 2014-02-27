#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.UserConfig.Core.Interfaces.TypeSpecific;

#endregion

namespace TrayGarden.Services.PlantServices.UserConfig.Core.TypeSpecific
{
  public class IntUserSetting : TypedUserSetting<int>, IIntUserSetting
  {
  }
}