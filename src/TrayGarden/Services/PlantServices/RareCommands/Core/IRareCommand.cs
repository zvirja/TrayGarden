using System;

namespace TrayGarden.Services.PlantServices.RareCommands.Core;

public interface IRareCommand
{
  Action ActionToPerform { get; }

  string Description { get; }

  string Title { get; }
}