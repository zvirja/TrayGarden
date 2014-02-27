#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
  public class SimpleDynamicStateProvider : IDynamicStateProvider
  {
    #region Public Events

    public event EventHandler RelevanceChanged;

    #endregion

    #region Public Properties

    public RelevanceLevel CurrentRelevanceLevel { get; set; }

    #endregion

    #region Public Methods and Operators

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

    #endregion
  }
}