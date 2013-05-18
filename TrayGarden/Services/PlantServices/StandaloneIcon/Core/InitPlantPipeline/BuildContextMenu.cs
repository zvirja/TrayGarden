using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Services.PlantServices.StandaloneIcon.Smorgasbord;

namespace TrayGarden.Services.PlantServices.StandaloneIcon.Core.InitPlantPipeline
{
    [UsedImplicitly]
    public class BuildContextMenu
    {
        [UsedImplicitly]
        public virtual void Process(InitPlantSIArgs args)
        {
            var contextMenu = args.SIBox.NotifyIcon.ContextMenuStrip ?? new ContextMenuStrip();
            var asContextMenuExtendable = args.PlantEx.Workhorse as IExtendContextMenu;
            if (asContextMenuExtendable != null)
                DrawInstanceSpecificContextMenu(contextMenu, asContextMenuExtendable);
            DrawInstanceIndependentContextMenu(contextMenu, args);
            args.SIBox.NotifyIcon.ContextMenuStrip = contextMenu;
        }

        protected virtual void DrawInstanceSpecificContextMenu(ContextMenuStrip contextMenu, IExtendContextMenu workInstance)
        {
            List<ToolStripMenuItem> contextMenuStrips = workInstance.GetStripsToAdd();
            if (contextMenuStrips == null || contextMenuStrips.Count == 0)
                return;
            foreach (ToolStripMenuItem contextMenuStrip in contextMenuStrips)
                contextMenu.Items.Add(contextMenuStrip);
            contextMenu.Items.Add("-");
        }

        protected virtual void DrawInstanceIndependentContextMenu(ContextMenuStrip contextMenu,
                                                                  InitPlantSIArgs args)
        {
            ToolStripItem closeComponent = contextMenu.Items.Add("Close component");
            closeComponent.Tag = args.SIBox;
            closeComponent.Click += args.CloseComponentClick;

            ToolStripItem exitGarden = contextMenu.Items.Add("Exit garden");
            exitGarden.Tag = args.SIBox;
            exitGarden.Click += args.ExitGardenClick;
        }



    }
}
