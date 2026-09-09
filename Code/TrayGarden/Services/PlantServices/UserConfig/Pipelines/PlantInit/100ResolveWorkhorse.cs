using JetBrains.Annotations;

using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception.Services;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.PlantInit;

[UsedImplicitly]
public class ResolveWorkhorse : IPipelineProcessor<InitPlantUCPipelineArg>
{
  [UsedImplicitly]
  public virtual void Process(InitPlantUCPipelineArg args)
  {
    var appropriateWorkhorse = args.RelatedPlant.GetFirstWorkhorseOfType<IUserConfiguration>();
    if (appropriateWorkhorse == null)
    {
      args.Abort();
      return;
    }
    args.Workhorse = appropriateWorkhorse;
  }
}
