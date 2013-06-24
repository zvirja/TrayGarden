using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.UI;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline
{
    public class CreateViewModel
    {
        public virtual void Process(GetMainVMPipelineArgs args)
        {
            args.ResultVM =  new WindowWithBackVMBase();
        }
    }
}
