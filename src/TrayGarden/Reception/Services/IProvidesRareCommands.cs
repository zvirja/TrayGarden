using System.Collections.Generic;
using TrayGarden.Services.PlantServices.RareCommands.Core;

namespace TrayGarden.Reception.Services;

public interface IProvidesRareCommands
{
  List<IRareCommand> GetRareCommands();
}