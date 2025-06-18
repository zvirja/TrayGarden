using JetBrains.Annotations;

using TrayGarden.UI.Configuration;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;

[UsedImplicitly]
public class ResolveContentVM
{
  [UsedImplicitly]
  public void Process(GetUCStepPipelineArgs args)
  {
    args.ConfigurationConstructInfo.ResultControlVM = new ConfigurationControlVM(
      args.ConfigurationConstructInfo.ConfigurationEntries,
      true);
  }
}