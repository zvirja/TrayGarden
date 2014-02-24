using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline
{
    [UsedImplicitly]
    public class CreateStep
    {
        [UsedImplicitly]
        public virtual void Process(GetApplicationConfigStepArgs args)
        {
            Assert.IsNotNull(args.StepConstructInfo.ContentVM != null, "args.StepConstructInfo.ContentVM is not null");
            WindowWithBackStateConstructInfo stepInfo = args.StepConstructInfo;
            stepInfo.ResultState = new WindowStepState(stepInfo.GlobalTitle,
                                                           stepInfo.Header,
                                                           stepInfo.ShortName,
                                                           stepInfo.ContentVM,
                                                           stepInfo.SuperAction,
                                                           stepInfo.StateSpecificHelpActions);
        }
    }
}
