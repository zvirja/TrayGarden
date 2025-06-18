using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.DynamicState
{
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
      stateProvider.RelevanceChanged += (sender, args) => this.EnqueStripForPendingUpdate(menuItem);
      this.EnqueStripForPendingUpdate(menuItem);
    }

    public virtual void BindToMenuStrip(ContextMenuStrip menuStrip)
    {
      menuStrip.Opening += this.MenuStripOnOpening;
    }

    [UsedImplicitly]
    public virtual void Initialize(IDynamicStateDecorator menuEntryDecorator)
    {
      this.MenuEntryDecorator = menuEntryDecorator;
      this.EntriesToUpdate = new HashSet<ExtendedToolStripMenuItem>();
    }

    protected virtual void EnqueStripForPendingUpdate(ExtendedToolStripMenuItem menuItem)
    {
      lock (this.lockObj)
      {
        this.EntriesToUpdate.Add(menuItem);
      }
    }

    protected virtual void MenuStripOnOpening(object sender, CancelEventArgs cancelEventArgs)
    {
      if (this.EntriesToUpdate.Count == 0)
      {
        return;
      }
      List<ExtendedToolStripMenuItem> copyOfEntries = null;
      lock (this.lockObj)
      {
        copyOfEntries = this.EntriesToUpdate.ToList();
        this.EntriesToUpdate.Clear();
      }
      foreach (ExtendedToolStripMenuItem item in copyOfEntries)
      {
        this.MenuEntryDecorator.DecorateStripItem(item, item.DymamicStateProvider.CurrentRelevanceLevel);
      }
    }
  }
}