#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
  public interface IDynamicStateDecorator
  {
    #region Public Methods and Operators

    void DecorateStripItem(ToolStripMenuItem menuItem, RelevanceLevel currentRelevanceLevel);

    #endregion
  }
}