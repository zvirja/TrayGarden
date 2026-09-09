using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolvePlantID : IPipelineProcessor<InitializePlantArgs>
{
  [UsedImplicitly]
  public virtual void Process(InitializePlantArgs args)
  {
    args.PlantID = args.PlantObject.GetType().FullName;
  }
}
