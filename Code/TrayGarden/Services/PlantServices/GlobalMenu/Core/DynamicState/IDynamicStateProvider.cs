using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
  public interface IDynamicStateProvider
  {
    event EventHandler RelevanceChanged;

    RelevanceLevel CurrentRelevanceLevel { get; }
  }
}