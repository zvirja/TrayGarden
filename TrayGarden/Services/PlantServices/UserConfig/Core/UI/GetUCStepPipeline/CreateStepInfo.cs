using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.Helpers;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserConfig.Core.UI.GetUCStepPipeline
{
    [UsedImplicitly]
    public class CreateStepInfo
    {

        public string ShortName { get; set; }
        public string Header { get; set; }
        public string GlobalTitle { get; set; }

        public CreateStepInfo()
        {
            ShortName = "user settings";
            Header = "User settings for plant";
            GlobalTitle = "Tray Garden -- User settings for #plantName plant";
        }

        [UsedImplicitly]
        public virtual void Process(GetUCStepPipelineArgs args)
        {
            Assert.IsNotNull(args.ConfigurationConstructInfo.ResultControlVM, "args.ConfigurationConstructInfo.ResultControlVM");
            WindowWithBackStateConstructInfo stateInfo = args.StateConstructInfo;
            stateInfo.ResultState = new WindowStepState(GetGlobalTitle(args),
                                                        stateInfo.Header ?? Header,
                                                        stateInfo.ShortName ?? ShortName,
                                                        args.ConfigurationConstructInfo.ResultControlVM,
                                                        stateInfo.SuperAction,
                                                        stateInfo.StateSpecificHelpActions);
        }

        protected virtual string GetGlobalTitle(GetUCStepPipelineArgs args)
        {
            var globalTitle = args.StateConstructInfo.GlobalTitle ?? GlobalTitle;
            return globalTitle.Replace("#plantName", args.UCServicePlantBox.RelatedPlantEx.Plant.HumanSupportingName);
        }
    }
}
