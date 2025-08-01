﻿using System;
using System.Drawing;
using System.Windows.Forms;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

[System.ComponentModel.DesignerCategory("")]
public class ExtendedToolStripMenuItem : ToolStripMenuItem
{
  public ExtendedToolStripMenuItem(string text, Image image, EventHandler onClick)
    : base(text, image, onClick)
  {
  }

  public IDynamicStateProvider DymamicStateProvider { get; set; }
}