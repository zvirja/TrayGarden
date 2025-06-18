using System.Collections.Generic;
using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.Services.PlantServices.RareCommands.Core;

public class RareCommandsServicePlantBox : ServicePlantBoxBase
{
  public RareCommandsServicePlantBox([NotNull] List<IRareCommand> rareCommands)
  {
    Assert.ArgumentNotNull(rareCommands, "rareCommands");
    this.RareCommands = rareCommands;
  }

  public List<IRareCommand> RareCommands { get; set; }
}