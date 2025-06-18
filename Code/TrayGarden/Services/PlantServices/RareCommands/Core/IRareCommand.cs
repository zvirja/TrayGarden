using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.RareCommands.Core;

public interface IRareCommand
{
  Action ActionToPerform { get; }

  string Description { get; }

  string Title { get; }
}