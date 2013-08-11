using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using JetBrains.Annotations;
using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Diagnostics;
using TrayGarden.Services.PlantServices.GlobalMenu.Core.UI.GetMainVMPipeline;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;
using TrayGarden.UI.Common;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Configuration.ApplicationConfiguration.Injection
{
    [UsedImplicitly]
    public class InjectApplicationConfigLink
    {
        [UsedImplicitly]
        public virtual void Process(GetMainVMPipelineArgs args)
        {
            args.SuperAction = GetSuperAction();
        }

        protected virtual ActionCommandVM GetSuperAction()
        {
            return new ActionCommandVM(new RelayCommand(ConfigureApplication, true), "Configure application");
        }

        protected virtual void ConfigureApplication(object obj)
        {
            WindowStepState applicationConfigStep = GetStateFromPipeline();
            if (applicationConfigStep == null)
            {
                HatcherGuide<IUIManager>.Instance.OKMessageBox("Application settings",
                                                               "Something is wrong. Tell app developer that he is stupid and the pipeline didn't return proper step object.",
                                                               MessageBoxImage.Error);
                Log.Warn("GetApplicationConfigStep pipeline hasn't returned proper object", this);
                return;
            }
            WindowWithBackVM.GoAheadWithBackIfPossible(applicationConfigStep);
        }

        protected virtual WindowStepState GetStateFromPipeline()
        {
            var args = new GetApplicationConfigStepArgs();
            GetApplicationConfigStep.Run(args);
            return args.Aborted ? null : args.Result as WindowStepState ?? args.StepConstructInfo.ResultState;
        }
    }
}
