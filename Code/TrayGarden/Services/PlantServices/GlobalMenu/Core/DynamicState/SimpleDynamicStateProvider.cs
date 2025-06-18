using System;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public class SimpleDynamicStateProvider : IDynamicStateProvider
{
  public event EventHandler RelevanceChanged;

  public RelevanceLevel CurrentRelevanceLevel { get; set; }

  public virtual void OnRelevanceChanged()
  {
    EventHandler handler = this.RelevanceChanged;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  public virtual void UpdateStateWithNotification(RelevanceLevel newLevel)
  {
    if (this.CurrentRelevanceLevel == newLevel)
    {
      return;
    }
    this.CurrentRelevanceLevel = newLevel;
    this.OnRelevanceChanged();
  }
}