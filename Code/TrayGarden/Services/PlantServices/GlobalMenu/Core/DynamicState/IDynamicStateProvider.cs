#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
  public interface IDynamicStateProvider
  {
    #region Public Events

    event EventHandler RelevanceChanged;

    #endregion

    #region Public Properties

    RelevanceLevel CurrentRelevanceLevel { get; }

    #endregion
  }
}