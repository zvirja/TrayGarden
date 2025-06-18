using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState;

public class DynamicStateWatcher : IDynamicStateWatcher
{
  protected object lockObj = new object();

  protected HashSet<ExtendedToolStripMenuItem> EntriesToUpdate { get; set; }

  protected IDynamicStateDecorator MenuEntryDecorator { get; set; }

  public virtual void AddStipToWatch(ExtendedToolStripMenuItem menuItem)
  {
    IDynamicStateProvider stateProvider = menuItem.DymamicStateProvider;
    if (stateProvider == null)
    {
      return;
    }
    stateProvider.RelevanceChanged += (sender, args) => EnqueStripForPendingUpdate(menuItem);
    EnqueStripForPendingUpdate(menuItem);
  }

  public virtual void BindToMenuStrip(ContextMenuStrip menuStrip)
  {
    menuStrip.Opening += MenuStripOnOpening;
  }

  [UsedImplicitly]
  public virtual void Initialize(IDynamicStateDecorator menuEntryDecorator)
  {
    MenuEntryDecorator = menuEntryDecorator;
    EntriesToUpdate = new HashSet<ExtendedToolStripMenuItem>();
  }

  protected virtual void EnqueStripForPendingUpdate(ExtendedToolStripMenuItem menuItem)
  {
    lock (lockObj)
    {
      EntriesToUpdate.Add(menuItem);
    }
  }

  protected virtual void MenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
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
      MenuEntryDecorator.DecorateStripItem(item, item.DymamicStateProvider.CurrentRelevanceLevel);
    }
  }
}