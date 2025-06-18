using System.Windows.Forms;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public interface IDynamicStateWatcher
{
  void AddStipToWatch(ExtendedToolStripMenuItem menuItem);

  void BindToMenuStrip(ContextMenuStrip menuStrip);
}