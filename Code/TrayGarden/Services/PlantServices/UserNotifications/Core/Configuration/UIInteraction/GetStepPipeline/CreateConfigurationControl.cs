using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class CreateConfigurationControl : IPipelineProcessor<UNConfigurationStepArgs>
{
  [UsedImplicitly]
  public virtual void Process(UNConfigurationStepArgs args)
  {
    args.ConfigurationConstructInfo.BuildControlVM();
  }
}
