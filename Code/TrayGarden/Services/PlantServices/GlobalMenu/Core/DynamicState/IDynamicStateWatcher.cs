#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
  public interface IDynamicStateWatcher
  {
    #region Public Methods and Operators

    void AddStipToWatch(ExtendedToolStripMenuItem menuItem);

    void BindToMenuStrip(ContextMenuStrip menuStrip);

    #endregion
  }
}