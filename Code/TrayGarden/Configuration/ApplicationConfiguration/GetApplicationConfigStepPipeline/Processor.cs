using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

[UsedImplicitly]
public class Processor : IPipelineProcessor<GetApplicationConfigStepArgs>
{
  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
  }
}
