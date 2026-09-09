using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Resources;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline;

[UsedImplicitly]
public class BuildContextMenu(IResourcesManager resourcesManager) : IPipelineProcessor<InitPlantSIArgs>
{
  public string ExitEntryIconResName { get; set; } = "exitIconV1";

  [UsedImplicitly]
  public virtual void Process(InitPlantSIArgs args)
  {
    var contextMenu = args.SIBox.NotifyIcon.ContextMenuStrip ?? new ContextMenuStrip();
    var asContextMenuExtendable = args.PlantEx.GetFirstWorkhorseOfType<IExtendContextMenu>();
    if (asContextMenuExtendable != null)
    {
      DrawInstanceSpecificContextMenu(contextMenu, asContextMenuExtendable);
    }
    DrawInstanceIndependentContextMenu(contextMenu, args);
    args.SIBox.NotifyIcon.ContextMenuStrip = contextMenu;
  }

  protected virtual void DrawInstanceIndependentContextMenu(ContextMenuStrip contextMenu, InitPlantSIArgs args)
  {
    /*
    ToolStripItem closeComponent = contextMenu.Items.Add("Hide plant");
    closeComponent.Tag = args.SIBox;
    closeComponent.Click += args.CloseComponentClick;
*/
    Icon exitIcon = resourcesManager.GetIconResource(ExitEntryIconResName, null);
    ToolStripItem exitGardenEntry;
    if (exitIcon != null)
    {
      exitGardenEntry = contextMenu.Items.Add("Exit garden", exitIcon.ToBitmap());
    }
    else
    {
      exitGardenEntry = contextMenu.Items.Add("Exit garden");
    }
    exitGardenEntry.Tag = args.SIBox;
    exitGardenEntry.Click += args.ExitGardenClick;
  }

  protected virtual void DrawInstanceSpecificContextMenu(ContextMenuStrip contextMenu, IExtendContextMenu workInstance)
  {
    List<ToolStripMenuItem> contextMenuStrips = workInstance.GetStripsToAdd();
    if (contextMenuStrips == null || contextMenuStrips.Count == 0)
    {
      return;
    }
    foreach (ToolStripMenuItem contextMenuStrip in contextMenuStrips)
    {
      contextMenu.Items.Add(contextMenuStrip);
    }
    contextMenu.Items.Add("-");
  }
}