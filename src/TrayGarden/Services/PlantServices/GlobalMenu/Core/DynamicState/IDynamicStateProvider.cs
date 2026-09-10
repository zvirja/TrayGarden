using System;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public interface IDynamicStateProvider
{
  event EventHandler RelevanceChanged;

  RelevanceLevel CurrentRelevanceLevel { get; }
}