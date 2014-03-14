#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

#endregion

namespace TrayGarden.Services.PlantServices.RareCommands.Core
{
  public class RareCommandsServicePlantBox : ServicePlantBoxBase
  {
    #region Constructors and Destructors

    public RareCommandsServicePlantBox([NotNull] List<IRareCommand> rareCommands)
    {
      Assert.ArgumentNotNull(rareCommands, "rareCommands");
      this.RareCommands = rareCommands;
    }

    #endregion

    #region Public Properties

    public List<IRareCommand> RareCommands { get; set; }

    #endregion
  }
}