#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;

#endregion

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ResolveSinglePlantVMPipeline
{
  [UsedImplicitly]
  public class CreatePlantVM
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(ResolveSinglePlantVMPipelineArgs args)
    {
      args.PlantVM = new SinglePlantVM();
      args.PlantVM.InitPlantVMWithPlantEx(args.PlantEx);
    }

    #endregion
  }
}