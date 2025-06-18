using JetBrains.Annotations;

namespace TrayGarden.Services.Engine.UI.GetStateForServicesConfigurationPipeline;

[UsedImplicitly]
public class InitializeGeneralSettings
{
  [UsedImplicitly]
  public virtual void Process(GetStateForServicesConfigurationPipelineArgs args)
  {
    args.ConfigConstructInfo.EnableResetAllOption = true;
    args.ConfigConstructInfo.AllowReboot = true;
  }
}