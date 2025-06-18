using System;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public class SimpleDynamicStateProvider : IDynamicStateProvider
{
  public event EventHandler RelevanceChanged;

  public RelevanceLevel CurrentRelevanceLevel { get; set; }

  public virtual void OnRelevanceChanged()
  {
    EventHandler handler = RelevanceChanged;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  public virtual void UpdateStateWithNotification(RelevanceLevel newLevel)
  {
    if (CurrentRelevanceLevel == newLevel)
    {
      return;
    }
    CurrentRelevanceLevel = newLevel;
    OnRelevanceChanged();
  }
}