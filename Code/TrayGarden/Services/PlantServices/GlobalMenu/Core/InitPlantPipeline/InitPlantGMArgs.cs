using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Media.Converters;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Plants;
using TrayGarden.Services.FleaMarket.IconChanger;
using TrayGarden.Services.PlantServices.StandaloneIcon.Core;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  public class InitPlantGMArgs : PipelineArgs
  {
    public bool IsNotifyIconChangerInUse { get; set; }
    public bool IsMenuExtendingInUse { get; set; }
    public bool IsAdvancedMenuExtendingInUse { get; set; }
    public string LuggageName { get; set; }
    public IPlantEx PlantEx { get; protected set; }
    public INotifyIconChangerMaster GlobalNotifyIconChanger { get; set; }

    public GlobalMenuPlantBox GMBox { get; set; }


    public InitPlantGMArgs([NotNull] IPlantEx plantEx, [NotNull] string luggageName,
                           [NotNull] INotifyIconChangerMaster globalNotifyIconChanger)
    {
      Assert.ArgumentNotNull(plantEx, "plantEx");
      Assert.ArgumentNotNull(luggageName, "luggageName");
      Assert.ArgumentNotNull(globalNotifyIconChanger, "globalNotifyIconChanger");
      PlantEx = plantEx;
      LuggageName = luggageName;
      GlobalNotifyIconChanger = globalNotifyIconChanger;
    }

    public virtual void AddToolStripItems(IEnumerable<ToolStripMenuItem> newItem)
    {
      if(GMBox.ToolStripMenuItems == null)
        GMBox.ToolStripMenuItems = new List<ToolStripItem>();
      GMBox.ToolStripMenuItems.AddRange(newItem);
    }


  }
}
