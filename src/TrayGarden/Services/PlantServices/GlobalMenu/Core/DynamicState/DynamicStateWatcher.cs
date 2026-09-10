using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public class DynamicStateWatcher : IDynamicStateWatcher
{
  private object lockObj = new object();

  public DynamicStateWatcher(IDynamicStateDecorator menuEntryDecorator)
  {
    MenuEntryDecorator = menuEntryDecorator;
    EntriesToUpdate = new HashSet<ExtendedToolStripMenuItem>();
  }

  private HashSet<ExtendedToolStripMenuItem> EntriesToUpdate { get; set; }

  private IDynamicStateDecorator MenuEntryDecorator { get; set; }

  public void AddStipToWatch(ExtendedToolStripMenuItem menuItem)
  {
    IDynamicStateProvider stateProvider = menuItem.DynamicStateProvider;
    if (stateProvider == null)
    {
      return;
    }
    stateProvider.RelevanceChanged += (sender, args) => EnqueStripForPendingUpdate(menuItem);
    EnqueStripForPendingUpdate(menuItem);
  }

  public void BindToMenuStrip(ContextMenuStrip menuStrip)
  {
    menuStrip.Opening += MenuStripOnOpening;
  }

  private void EnqueStripForPendingUpdate(ExtendedToolStripMenuItem menuItem)
  {
    lock (lockObj)
    {
      EntriesToUpdate.Add(menuItem);
    }
  }

  private void MenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
  {
    if (EntriesToUpdate.Count == 0)
    {
      return;
    }
    List<ExtendedToolStripMenuItem> copyOfEntries = null;
    lock (lockObj)
    {
      copyOfEntries = EntriesToUpdate.ToList();
      EntriesToUpdate.Clear();
    }
    foreach (ExtendedToolStripMenuItem item in copyOfEntries)
    {
      MenuEntryDecorator.DecorateStripItem(item, item.DynamicStateProvider.CurrentRelevanceLevel);
    }
  }
}