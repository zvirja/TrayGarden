using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

[UsedImplicitly]
public class InitializeGeneralSettings : IPipelineProcessor<GetStateForServicesConfigurationPipelineArgs>
{
  [UsedImplicitly]
  public void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    args.ConfigConstructInfo.EnableResetAllOption = true;
    args.ConfigConstructInfo.AllowReboot = true;
  }
}
