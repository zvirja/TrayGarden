#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
  public class DynamicStateDecorator : IDynamicStateDecorator
  {
    #region Public Methods and Operators

    public void DecorateStripItem(ToolStripMenuItem menuItem, RelevanceLevel currentRelevanceLevel)
    {
      switch (currentRelevanceLevel)
      {
        case RelevanceLevel.Irrelevant:
          this.SetToIrrelevantState(menuItem);
          break;
        case RelevanceLevel.Low:
          this.SetToLowState(menuItem);
          break;
        case RelevanceLevel.Normal:
          this.SetToNormalState(menuItem);
          break;
        case RelevanceLevel.High:
          this.SetToHighState(menuItem);
          break;
      }
    }

    #endregion

    #region Methods

    protected virtual void SetToHighState(ToolStripMenuItem menuItem)
    {
      menuItem.ForeColor = Color.Black;
      menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style | FontStyle.Bold);
      menuItem.Enabled = true;
    }

    protected virtual void SetToIrrelevantState(ToolStripMenuItem menuItem)
    {
      menuItem.ForeColor = Color.Black;
      menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style & ~FontStyle.Bold);
      menuItem.Enabled = false;
    }

    protected virtual void SetToLowState(ToolStripMenuItem menuItem)
    {
      menuItem.ForeColor = Color.DarkGray;
      menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style & ~FontStyle.Bold);
      menuItem.Enabled = true;
    }

    protected virtual void SetToNormalState(ToolStripMenuItem menuItem)
    {
      menuItem.ForeColor = Color.Black;
      menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style & ~FontStyle.Bold);
      menuItem.Enabled = true;
    }

    #endregion
  }
}