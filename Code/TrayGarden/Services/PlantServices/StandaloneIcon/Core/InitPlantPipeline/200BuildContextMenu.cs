#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Reception.Services.StandaloneIcon;
using TrayGarden.Resources;
using TrayGarden.TypesHatcher;

#endregion

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class BuildContextMenu
  {
    #region Constructors and Destructors

    public BuildContextMenu()
    {
      this.ExitEntryIconResName = "exitIconV1";
    }

    #endregion

    #region Public Properties

    public string ExitEntryIconResName { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantSIArgs args)
    {
      var contextMenu = args.SIBox.NotifyIcon.ContextMenuStrip ?? new ContextMenuStrip();
      var asContextMenuExtendable = args.PlantEx.GetFirstWorkhorseOfType<IExtendContextMenu>();
      if (asContextMenuExtendable != null)
      {
        this.DrawInstanceSpecificContextMenu(contextMenu, asContextMenuExtendable);
      }
      this.DrawInstanceIndependentContextMenu(contextMenu, args);
      args.SIBox.NotifyIcon.ContextMenuStrip = contextMenu;
    }

    #endregion

    #region Methods

    protected virtual void DrawInstanceIndependentContextMenu(ContextMenuStrip contextMenu, InitPlantSIArgs args)
    {
      /*
      ToolStripItem closeComponent = contextMenu.Items.Add("Hide plant");
      closeComponent.Tag = args.SIBox;
      closeComponent.Click += args.CloseComponentClick;
*/
      Icon exitIcon = HatcherGuide<IResourcesManager>.Instance.GetIconResource(this.ExitEntryIconResName, null);
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

    #endregion
  }
}