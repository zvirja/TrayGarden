using JetBrains.Annotations;

using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline;

public class CreateViewModel
{
  [UsedImplicitly]
  public virtual void Process(GetMainVMPipelineArgs args)
  {
    args.ResultVM = new WindowWithBackVM();
  }
}