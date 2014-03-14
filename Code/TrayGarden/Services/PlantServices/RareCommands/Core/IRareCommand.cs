#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.RareCommands.Core
{
  public interface IRareCommand
  {
    #region Public Properties

    Action ActionToPerform { get; }

    string Description { get; }

    string Title { get; }

    #endregion
  }
}