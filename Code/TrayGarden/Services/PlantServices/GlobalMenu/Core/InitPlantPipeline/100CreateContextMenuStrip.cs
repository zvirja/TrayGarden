#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Reception.Services;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.ContextMenuCollecting;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.InitPlantPipeline
{
  [UsedImplicitly]
  public class CreateContextMenuStrip
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(InitPlantGMArgs args)
    {
      var asExpected = args.PlantEx.GetFirstWorkhorseOfType<IExtendsGlobalMenu>();
      if (asExpected == null)
      {
        return;
      }
      var contextMenuAppender = new MenuEntriesAppender();
      if (!asExpected.FillProvidedContextMenuBuilder(contextMenuAppender))
      {
        return;
      }
      if (contextMenuAppender.OutputItems.Count == 0)
      {
        return;
      }
      args.IsMenuExtendingInUse = true;
      args.AddToolStripItems(contextMenuAppender.OutputItems);
    }

    #endregion
  }
}