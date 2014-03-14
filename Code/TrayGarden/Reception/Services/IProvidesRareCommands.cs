#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.Services.PlantServices.RareCommands.Core;

#endregion

namespace TrayGarden.Reception.Services
{
  public interface IProvidesRareCommands
  {
    #region Public Methods and Operators

    List<IRareCommand> GetRareCommands();

    #endregion
  }
}