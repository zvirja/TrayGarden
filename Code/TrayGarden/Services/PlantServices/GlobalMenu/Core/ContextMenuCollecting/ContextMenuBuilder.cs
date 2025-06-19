using System;
using System.Collections.Generic;
using System.Drawing;
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
    ConfigureIconResourceName = "configureV1";
    ExitIconResourceName = "exitIconV1";
    BoldMainMenuEntries = true;
    ItalicMainMenuEntries = true;
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
    contextMenuStrip.AutoSize = true;
    
    BuildContextMenuPrefix(contextMenuStrip);
    EnumeratePlantBoxes(plantBoxes, contextMenuStrip, dynamicStateWatcher);
    BuildContextMenuSuffix(contextMenuStrip);
    dynamicStateWatcher.BindToMenuStrip(contextMenuStrip);
    return contextMenuStrip;
  }

  protected virtual void BuildContextMenuPrefix(ContextMenuStrip contextMenuStrip)
  {
    var configureItem = contextMenuStrip.Items.Add("Configure");
    Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(ConfigureIconResourceName, null);
    if (iconResource != null)
    {
      configureItem.Image = iconResource.ToBitmap();
    }
    configureItem.Font = new Font(configureItem.Font, GetMainMenuEntriesStyle());
    configureItem.Click += delegate(object sender, EventArgs args)
    {
      if (ConfigureContextItemOnClick != null)
      {
        ConfigureContextItemOnClick(sender, args);
      }
    };
    contextMenuStrip.Items.Add("-");
  }

  protected virtual void BuildContextMenuSuffix(ContextMenuStrip contextMenuStrip)
  {
    var exitItem = contextMenuStrip.Items.Add("Exit Garden");
    Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(ExitIconResourceName, null);
    if (iconResource != null)
    {
      exitItem.Image = iconResource.ToBitmap();
    }
    exitItem.Font = new Font(exitItem.Font, GetMainMenuEntriesStyle());
    exitItem.Click += delegate(object sender, EventArgs args)
    {
      if (ExitContextItemOnClick != null)
      {
        ExitContextItemOnClick(sender, args);
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
    if (BoldMainMenuEntries)
    {
      result |= FontStyle.Bold;
    }
    if (ItalicMainMenuEntries)
    {
      result |= FontStyle.Italic;
    }
    return result;
  }
}