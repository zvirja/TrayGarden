using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.FleaMarket.IconChanger;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline;

public class InitPlantGMArgs : PipelineArgs
{
  public InitPlantGMArgs(
    [NotNull] IPlantEx plantEx,
    [NotNull] string luggageName,
    [NotNull] INotifyIconChangerMaster globalNotifyIconChanger)
  {
    Assert.ArgumentNotNull(plantEx, "plantEx");
    Assert.ArgumentNotNull(luggageName, "luggageName");
    Assert.ArgumentNotNull(globalNotifyIconChanger, "globalNotifyIconChanger");
    this.PlantEx = plantEx;
    this.LuggageName = luggageName;
    this.GlobalNotifyIconChanger = globalNotifyIconChanger;
  }

  public GlobalMenuPlantBox GMBox { get; set; }

  public INotifyIconChangerMaster GlobalNotifyIconChanger { get; set; }

  public bool IsAdvancedMenuExtendingInUse { get; set; }

  public bool IsMenuExtendingInUse { get; set; }

  public bool IsNotifyIconChangerInUse { get; set; }

  public string LuggageName { get; set; }

  public IPlantEx PlantEx { get; protected set; }

  public virtual void AddToolStripItems(IEnumerable<ToolStripMenuItem> newItem)
  {
    if (this.GMBox.ToolStripMenuItems == null)
    {
      this.GMBox.ToolStripMenuItems = new List<ToolStripItem>();
    }
    this.GMBox.ToolStripMenuItems.AddRange(newItem);
  }
}