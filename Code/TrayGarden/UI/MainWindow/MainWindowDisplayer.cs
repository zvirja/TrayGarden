using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using TrayGarden.Diagnostics;
using TrayGarden.TypesHatcher;
using TrayGarden.UI.MainWindow.ResolveVMPipeline;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.MainWindow
{
  public class MainWindowDisplayer : IMainWindowDisplayer
  {
    public virtual void PopupMainWindow()
    {
      IWindowWithBack windowWithBack = HatcherGuide<IWindowWithBack>.Instance;
      Assert.IsNotNull(windowWithBack, "Window with back wasn't resolved");
      if (windowWithBack.IsCurrentlyDisplayed)
      {
        windowWithBack.BringToFront();
        return;
      }
      WindowWithBackVM mainWindowVM = GetMainVMPipelineRunner.Run(new GetMainVMPipelineArgs());
      if (mainWindowVM == null)
      {
        HatcherGuide<IUIManager>.Instance.OKMessageBox(
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
}