using System.Drawing;
using System.Windows.Forms;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public class DynamicStateDecorator : IDynamicStateDecorator
{
  public void DecorateStripItem(ToolStripMenuItem menuItem, RelevanceLevel currentRelevanceLevel)
  {
    switch (currentRelevanceLevel)
    {
      case RelevanceLevel.Irrelevant:
        SetToIrrelevantState(menuItem);
        break;
      case RelevanceLevel.Low:
        SetToLowState(menuItem);
        break;
      case RelevanceLevel.Normal:
        SetToNormalState(menuItem);
        break;
      case RelevanceLevel.High:
        SetToHighState(menuItem);
        break;
    }
  }

  private void SetToHighState(ToolStripMenuItem menuItem)
  {
    menuItem.ForeColor = Color.Black;
    menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style | FontStyle.Bold);
    menuItem.Enabled = true;
  }

  private void SetToIrrelevantState(ToolStripMenuItem menuItem)
  {
    menuItem.ForeColor = Color.Black;
    menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style & ~FontStyle.Bold);
    menuItem.Enabled = false;
  }

  private void SetToLowState(ToolStripMenuItem menuItem)
  {
    menuItem.ForeColor = Color.DarkGray;
    menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style & ~FontStyle.Bold);
    menuItem.Enabled = true;
  }

  private void SetToNormalState(ToolStripMenuItem menuItem)
  {
    menuItem.ForeColor = Color.Black;
    menuItem.Font = new Font(menuItem.Font, menuItem.Font.Style & ~FontStyle.Bold);
    menuItem.Enabled = true;
  }
}