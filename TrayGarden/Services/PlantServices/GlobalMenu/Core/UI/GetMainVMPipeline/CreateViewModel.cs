using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.UI;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline
{
    public class CreateViewModel
    {
        public virtual void Process(GetMainVMPipelineArgs args)
        {
            args.ResultVM =  new WindowWithBackVM();
        }
    }
}
