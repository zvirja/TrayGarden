using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.ViewModels;
using TrayGarden.UI.WindowWithBackStuff;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline
{
    public class InitializeFirstStep
    {
        public string GlobalTitle { get; set; }
        public string ShortName { get; set; }
        public string Header { get; set; }

        public virtual void Process(GetMainVMPipelineArgs args)
        {
            Assert.IsNotNull(args.ResultVM,"Result VM can't be null");
            Assert.IsNotNull(args.PlantsConfigVM,"PlantsConfig VM can't be null");
            args.ResultVM.ReplaceInitialState(GetInitialStep(args.PlantsConfigVM));
        }

        protected virtual WindowWithBackState GetInitialStep(PlantsConfigVM plantsConfigVM)
        {
            var step = new WindowWithBackState(GlobalTitle.GetValueOrDefault("Tray Garden -- Plants configuration"),
                                               Header.GetValueOrDefault("Here you configure plants"),
                                               ShortName.GetValueOrDefault("plants config"), plantsConfigVM, null, null);
            return step;
        }


        
    }
}
