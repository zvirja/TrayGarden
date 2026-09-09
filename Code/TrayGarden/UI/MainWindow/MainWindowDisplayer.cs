using System.Windows;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.MainWindow.ResolveVMPipeline;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.MainWindow;

public class MainWindowDisplayer(IWindowWithBack windowWithBack, IUIManager uiManager, IPipelineRunner pipelineRunner)
  : IMainWindowDisplayer
{
  public virtual void PopupMainWindow()
  {
    Assert.IsNotNull(windowWithBack, "Window with back wasn't resolved");
    if (windowWithBack.IsCurrentlyDisplayed)
    {
      windowWithBack.BringToFront();
      return;
    }
    var pipelineArgs = new GetMainVMPipelineArgs();
    pipelineRunner.Run(pipelineArgs);
    WindowWithBackVM mainWindowVM = pipelineArgs.Aborted ? null : pipelineArgs.ResultVM;
    if (mainWindowVM == null)
    {
      uiManager.OKMessageBox(
        "Plant configuration",
        "We was unable to resolve main View Model. Please provide log files to developer",
        MessageBoxImage.Error);
    }
    else
    {
      windowWithBack.PrepareAndShow(mainWindowVM);
    }
  }
}
