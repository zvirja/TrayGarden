#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting
{
  [System.ComponentModel.DesignerCategory("")]
  public class ExtendedToolStripMenuItem : ToolStripMenuItem
  {
    #region Constructors and Destructors

    public ExtendedToolStripMenuItem(string text, Image image, EventHandler onClick)
      : base(text, image, onClick)
    {
    }

    #endregion

    #region Public Properties

    public IDynamicStateProvider DymamicStateProvider { get; set; }

    #endregion
  }
}