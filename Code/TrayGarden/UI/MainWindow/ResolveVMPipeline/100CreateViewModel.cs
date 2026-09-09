using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.RuntimeSettings;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.UI.MainWindow.ResolveVMPipeline;

public class CreateViewModel(IRuntimeSettingsManager runtimeSettingsManager, IUIManager uiManager)
  : IPipelineProcessor<GetMainVMPipelineArgs>
{
  [UsedImplicitly]
  public virtual void Process(GetMainVMPipelineArgs args)
  {
    args.ResultVM = new WindowWithBackVM(runtimeSettingsManager, uiManager);
  }
}
