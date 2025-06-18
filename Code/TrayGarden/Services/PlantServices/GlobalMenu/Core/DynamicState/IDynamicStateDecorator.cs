using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
  public interface IDynamicStateDecorator
  {
    void DecorateStripItem(ToolStripMenuItem menuItem, RelevanceLevel currentRelevanceLevel);
  }
}