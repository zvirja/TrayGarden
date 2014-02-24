#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Diagnostics;
using TrayGarden.TypesHatcher;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.MainWindow.ResolveVMPipeline;
using TrayGarden.UI.WindowWithReturn;

#endregion

namespace TrayGarden.Configuration.ApplicationConfiguration.Injection
{
  [UsedImplicitly]
  public class InjectApplicationConfigLink
  {
    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Process(GetMainVMPipelineArgs args)
    {
      args.SuperAction = this.GetSuperAction();
    }

    #endregion

    #region Methods

    protected virtual void ConfigureApplication(object obj)
    {
      WindowStepState applicationConfigStep = this.GetStateFromPipeline();
      if (applicationConfigStep == null)
      {
        HatcherGuide<IUIManager>.Instance.OKMessageBox(
          "Application settings",
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

    protected virtual ActionCommandVM GetSuperAction()
    {
      return new ActionCommandVM(new RelayCommand(this.ConfigureApplication, true), "Configure application");
    }

    #endregion
  }
}