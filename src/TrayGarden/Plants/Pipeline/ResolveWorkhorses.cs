using System.Collections.Generic;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Pipelines.Engine;
using TrayGarden.Reception;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolveWorkhorses : IPipelineProcessor<InitializePlantArgs>
{
  [UsedImplicitly]
  public void Process(InitializePlantArgs args)
  {
    var workhorses = new List<object> { args.PlantObject };
    var asExpected = args.PlantObject as IServicesDelegation;
    if (asExpected != null)
    {
      List<object> workhorseCandidates = asExpected.GetServiceDelegates();
      if (workhorseCandidates != null)
      {
        workhorses.AddRange(workhorseCandidates);
      }
      Log.For(this).Debug("Plant {PlantType} supports service delegation", args.PlantObject.GetType().FullName);
    }
    args.Workhorses = workhorses;
  }
}
