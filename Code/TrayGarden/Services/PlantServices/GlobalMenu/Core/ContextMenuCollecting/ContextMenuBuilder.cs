using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

using Microsoft.Extensions.Options;

using TrayGarden.Configuration.Options;
using TrayGarden.Resources;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

[UsedImplicitly]
public class ContextMenuBuilder(IResourcesManager resourcesManager, IOptions<TrayGardenOptions> options)
{
  private readonly ContextMenuOptions _cfg = options.Value.GlobalMenu.ContextMenu;

  public bool BoldMainMenuEntries => _cfg.BoldMainMenuEntries;

  public EventHandler ConfigureContextItemOnClick { get; set; }

  public string ConfigureIconResourceName => _cfg.ConfigureIconResourceName;

  public EventHandler ExitContextItemOnClick { get; set; }

  public string ExitIconResourceName => _cfg.ExitIconResourceName;

  public bool InsertDelimiterBetweenPlants => _cfg.InsertDelimiterBetweenPlants;

  public bool ItalicMainMenuEntries => _cfg.ItalicMainMenuEntries;

  public ContextMenuStrip BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes, IDynamicStateWatcher dynamicStateWatcher)
  {
    var contextMenuStrip = new ContextMenuStrip();
    contextMenuStrip.AutoSize = true;

    BuildContextMenuPrefix(contextMenuStrip);
    EnumeratePlantBoxes(plantBoxes, contextMenuStrip, dynamicStateWatcher);
    BuildContextMenuSuffix(contextMenuStrip);
    dynamicStateWatcher.BindToMenuStrip(contextMenuStrip);
    return contextMenuStrip;
  }

  private void BuildContextMenuPrefix(ContextMenuStrip contextMenuStrip)
  {
    var configureItem = contextMenuStrip.Items.Add("Configure");
    Icon iconResource = resourcesManager.GetIconResource(ConfigureIconResourceName, null);
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

  private void BuildContextMenuSuffix(ContextMenuStrip contextMenuStrip)
  {
    var exitItem = contextMenuStrip.Items.Add("Exit Garden");
    Icon iconResource = resourcesManager.GetIconResource(ExitIconResourceName, null);
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

  private void EnumeratePlantBoxes(List<GlobalMenuPlantBox> plantBoxes, ContextMenuStrip menuStrip, IDynamicStateWatcher dynamicStateWatcher)
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

  private FontStyle GetMainMenuEntriesStyle()
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
