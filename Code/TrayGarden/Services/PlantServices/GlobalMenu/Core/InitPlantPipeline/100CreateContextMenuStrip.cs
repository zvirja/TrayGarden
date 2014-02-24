using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using TrayGarden.Reception.Services;
using TrayGarden.Helpers;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class CreateContextMenuStrip
  {
    [UsedImplicitly]
    public virtual void Process(InitPlantGMArgs args)
    {
      var asExpected = args.PlantEx.GetFirstWorkhorseOfType<IExtendsGlobalMenu>();
      if (asExpected == null)
        return;
      var contextMenuAppender = new MenuEntriesAppender();
      if (!asExpected.FillProvidedContextMenuBuilder(contextMenuAppender))
        return;
      if (contextMenuAppender.OutputItems.Count == 0)
        return;
      args.IsMenuExtendingInUse = true;
      args.AddToolStripItems(contextMenuAppender.OutputItems);
    }

    
  }
}
