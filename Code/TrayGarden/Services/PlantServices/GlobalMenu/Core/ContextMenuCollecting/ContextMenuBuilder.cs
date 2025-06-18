using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Resources;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

[UsedImplicitly]
public class ContextMenuBuilder
{
  public ContextMenuBuilder()
  {
    this.ConfigureIconResourceName = "configureV1";
    this.ExitIconResourceName = "exitIconV1";
    this.BoldMainMenuEntries = true;
    this.ItalicMainMenuEntries = true;
  }

  public bool BoldMainMenuEntries { get; set; }

  public EventHandler ConfigureContextItemOnClick { get; set; }

  public string ConfigureIconResourceName { get; set; }

  public EventHandler ExitContextItemOnClick { get; set; }

  public string ExitIconResourceName { get; set; }

  public bool InsertDelimiterBetweenPlants { get; set; }

  public bool ItalicMainMenuEntries { get; set; }

  public virtual ContextMenuStrip BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes, IDynamicStateWatcher dynamicStateWatcher)
  {
    var contextMenuStrip = new ContextMenuStrip();
    this.BuildContextMenuPrefix(contextMenuStrip);
    this.EnumeratePlantBoxes(plantBoxes, contextMenuStrip, dynamicStateWatcher);
    this.BuildContextMenuSuffix(contextMenuStrip);
    dynamicStateWatcher.BindToMenuStrip(contextMenuStrip);
    return contextMenuStrip;
  }

  protected virtual void BuildContextMenuPrefix(ContextMenuStrip contextMenuStrip)
  {
    var configureItem = contextMenuStrip.Items.Add("Configure");
    Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(this.ConfigureIconResourceName, null);
    if (iconResource != null)
    {
      configureItem.Image = iconResource.ToBitmap();
    }
    configureItem.Font = new Font(configureItem.Font, this.GetMainMenuEntriesStyle());
    configureItem.Click += delegate(object sender, EventArgs args)
    {
      if (this.ConfigureContextItemOnClick != null)
      {
        this.ConfigureContextItemOnClick(sender, args);
      }
    };
    contextMenuStrip.Items.Add("-");
  }

  protected virtual void BuildContextMenuSuffix(ContextMenuStrip contextMenuStrip)
  {
    var exitItem = contextMenuStrip.Items.Add("Exit Garden");
    Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(this.ExitIconResourceName, null);
    if (iconResource != null)
    {
      exitItem.Image = iconResource.ToBitmap();
    }
    exitItem.Font = new Font(exitItem.Font, this.GetMainMenuEntriesStyle());
    exitItem.Click += delegate(object sender, EventArgs args)
    {
      if (this.ExitContextItemOnClick != null)
      {
        this.ExitContextItemOnClick(sender, args);
      }
    };
  }

  protected virtual void EnumeratePlantBoxes(List<GlobalMenuPlantBox> plantBoxes, ContextMenuStrip menuStrip, IDynamicStateWatcher dynamicStateWatcher)
  {
    foreach (GlobalMenuPlantBox globalMenuPlantBox in plantBoxes)
    {
      if (globalMenuPlantBox.ToolStripMenuItems != null && globalMenuPlantBox.ToolStripMenuItems.Count > 0)
      {
        foreach (ToolStripItem contextMenuItem in globalMenuPlantBox.ToolStripMenuItems)
        {
          menuStrip.Items.Add(contextMenuItem);
          var extendedMenuStrip = contextMenuItem as ExtendedToolStripMenuItem;
          if (extendedMenuStrip != null)
          {
            dynamicStateWatcher.AddStipToWatch(extendedMenuStrip);
          }
        }
      }
    }
  }

  protected virtual FontStyle GetMainMenuEntriesStyle()
  {
    FontStyle result = 0;
    if (this.BoldMainMenuEntries)
    {
      result |= FontStyle.Bold;
    }
    if (this.ItalicMainMenuEntries)
    {
      result |= FontStyle.Italic;
    }
    return result;
  }
}