using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.RareCommands.Core;

namespace TrayGarden.Reception.Services;

public interface IProvidesRareCommands
{
  List<IRareCommand> GetRareCommands();
}