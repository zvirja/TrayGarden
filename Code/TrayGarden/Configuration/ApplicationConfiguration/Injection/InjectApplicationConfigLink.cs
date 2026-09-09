using System.Windows;

using JetBrains.Annotations;

using TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;
using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI;
using TrayGarden.UI.Common.Commands;
using TrayGarden.UI.MainWindow.ResolveVMPipeline;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Configuration.ApplicationConfiguration.Injection;

[UsedImplicitly]
public class InjectApplicationConfigLink(IPipelineRunner pipelineRunner, IUIManager uiManager)
  : IPipelineProcessor<GetMainVMPipelineArgs>
{
  [UsedImplicitly]
  public void Process(GetMainVMPipelineArgs args)
  {
    args.SuperAction = GetSuperAction();
  }

  private void ConfigureApplication(object obj)
  {
    WindowStepState applicationConfigStep = GetStateFromPipeline();
    if (applicationConfigStep == null)
    {
      uiManager.OKMessageBox(
        "Application settings",
        "Something is wrong. Tell app developer that he is stupid and the pipeline didn't return proper step object.",
        MessageBoxImage.Error);
      Log.For(this).Warning("GetApplicationConfigStep pipeline hasn't returned proper object");
      return;
    }
    WindowWithBackVM.GoAheadWithBackIfPossible(applicationConfigStep);
  }

  private WindowStepState GetStateFromPipeline()
  {
    var args = new GetApplicationConfigStepArgs();
    pipelineRunner.Run(args);
    return args.Aborted ? null : args.Result as WindowStepState ?? args.StepConstructInfo.ResultState;
  }

  private ActionCommandVM GetSuperAction()
  {
    return new ActionCommandVM(new RelayCommand(ConfigureApplication, true), "Configure application");
  }
}
