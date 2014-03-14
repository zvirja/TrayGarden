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
  public class SimpleRareCommand : IRareCommand
  {
    #region Constructors and Destructors

    public SimpleRareCommand([NotNull] string title, [NotNull] string description, [NotNull] Action actionToPerform)
    {
      Assert.ArgumentNotNullOrEmpty(title, "title");
      Assert.ArgumentNotNullOrEmpty(description, "description");
      Assert.ArgumentNotNull(actionToPerform, "actionToPerform");
      this.Title = title;
      this.Description = description;
      this.ActionToPerform = actionToPerform;
    }

    #endregion

    #region Public Properties

    public Action ActionToPerform { get; protected set; }

    public string Description { get; protected set; }

    public string Title { get; protected set; }

    #endregion
  }
}