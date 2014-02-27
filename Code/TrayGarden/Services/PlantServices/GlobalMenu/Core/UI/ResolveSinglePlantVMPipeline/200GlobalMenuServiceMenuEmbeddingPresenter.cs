#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Plants;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
  [UsedImplicitly]
  public class GlobalMenuServiceMenuEmbeddingPresenter : ServicePresenterBase<GlobalMenuService>
  {
    #region Constructors and Destructors

    public GlobalMenuServiceMenuEmbeddingPresenter()
    {
      this.ServiceName = "Embedding to global menu";
      this.ServiceDescription = "If service is enabled, plant is enabled to embed its row to global menu.";
    }

    #endregion

    #region Methods

    protected override ServiceForPlantVMBase GetServiceVM(GlobalMenuService serviceInstance, IPlantEx plantEx)
    {
      var luggage = serviceInstance.GetPlantLuggage(plantEx);
      if (luggage.ToolStripMenuItems == null)
      {
        return null;
      }
      return new ServiceForPlantWithEnablingPlantBoxBasedVM(this.ServiceName, this.ServiceDescription, luggage);
    }

    #endregion
  }
}