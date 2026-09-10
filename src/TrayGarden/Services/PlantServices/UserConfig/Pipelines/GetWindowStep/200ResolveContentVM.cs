using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;

[UsedImplicitly]
public class ResolveContentVM : IPipelineProcessor<GetUCStepPipelineArgs>
{
  [UsedImplicitly]
  public void Process(GetUCStepPipelineArgs args)
  {
    args.ConfigurationConstructInfo.ResultControlVM = new ConfigurationControlVM(
      args.ConfigurationConstructInfo.ConfigurationEntries,
      true);
  }
}
