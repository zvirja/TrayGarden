using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;

namespace TrayGarden.Services.PlantServices.RareCommands.Core;

public class SimpleRareCommand : IRareCommand
{
  public SimpleRareCommand([NotNull] string title, [NotNull] string description, [NotNull] Action actionToPerform)
  {
    Assert.ArgumentNotNullOrEmpty(title, "title");
    Assert.ArgumentNotNullOrEmpty(description, "description");
    Assert.ArgumentNotNull(actionToPerform, "actionToPerform");
    this.Title = title;
    this.Description = description;
    this.ActionToPerform = actionToPerform;
  }

  public Action ActionToPerform { get; protected set; }

  public string Description { get; protected set; }

  public string Title { get; protected set; }
}