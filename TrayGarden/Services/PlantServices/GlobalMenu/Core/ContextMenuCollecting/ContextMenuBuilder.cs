using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Resources;
using TrayGarden.TypesHatcher;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting
{
  [UsedImplicitly]
  public class ContextMenuBuilder
  {
    public EventHandler ExitContextItemOnClick { get; set; }
    public EventHandler ConfigureContextItemOnClick { get; set; }

    public string ConfigureIconResourceName { get; set; }
    public string ExitIconResourceName { get; set; }
    public bool BoldMainMenuEntries { get; set; }
    public bool ItalicMainMenuEntries { get; set; }
    public bool InsertDemiliterBetweenPlants { get; set; }

    public ContextMenuBuilder()
    {
      ConfigureIconResourceName = "configureV1";
      ExitIconResourceName = "exitIconV1";
      BoldMainMenuEntries = true;
      ItalicMainMenuEntries = true;
    }

    public virtual ContextMenuStrip BuildContextMenu(List<GlobalMenuPlantBox> plantBoxes)
    {
      var contextMenuStrip = new ContextMenuStrip();
      BuildContextMenuPrefix(contextMenuStrip);
      EnumeratePlantBoxes(plantBoxes, contextMenuStrip);
      BuildContextMenuSuffix(contextMenuStrip);
      return contextMenuStrip;
    }

    protected virtual void BuildContextMenuPrefix(ContextMenuStrip contextMenuStrip)
    {
      var configureItem = contextMenuStrip.Items.Add("Configure");
      Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(ConfigureIconResourceName, null);
      if (iconResource != null)
        configureItem.Image = iconResource.ToBitmap();
      configureItem.Font = new Font(configureItem.Font, GetMainMenuEntriesStyle());
      configureItem.Click += delegate(object sender, EventArgs args)
      {
        if (ConfigureContextItemOnClick != null)
          ConfigureContextItemOnClick(sender, args);
      };
      contextMenuStrip.Items.Add("-");
    }

    protected virtual FontStyle GetMainMenuEntriesStyle()
    {
      FontStyle result = 0;
      if (BoldMainMenuEntries)
        result |= FontStyle.Bold;
      if (ItalicMainMenuEntries)
        result |= FontStyle.Italic;
      return result;
    }

    protected virtual void BuildContextMenuSuffix(ContextMenuStrip contextMenuStrip)
    {
      contextMenuStrip.Items.Add("-");
      var exitItem = contextMenuStrip.Items.Add("Exit Garden");
      Icon iconResource = HatcherGuide<IResourcesManager>.Instance.GetIconResource(ExitIconResourceName, null);
      if (iconResource != null)
        exitItem.Image = iconResource.ToBitmap();
      exitItem.Font = new Font(exitItem.Font, GetMainMenuEntriesStyle());
      exitItem.Click += delegate(object sender, EventArgs args)
      {
        if (ExitContextItemOnClick != null)
          ExitContextItemOnClick(sender, args);
      };
    }

    protected virtual void EnumeratePlantBoxes(List<GlobalMenuPlantBox> plantBoxes, ContextMenuStrip menuStrip)
    {
      bool valueWasAppended = false;
      foreach (GlobalMenuPlantBox globalMenuPlantBox in plantBoxes)
        if (globalMenuPlantBox.ToolStripMenuItems != null && globalMenuPlantBox.ToolStripMenuItems.Count > 0)
        {
          if (valueWasAppended && InsertDemiliterBetweenPlants)
            menuStrip.Items.Add("-");
          foreach (ToolStripMenuItem contextMenuItem in globalMenuPlantBox.ToolStripMenuItems)
            menuStrip.Items.Add(contextMenuItem);
          valueWasAppended = true;
        }
    }

  }
}
