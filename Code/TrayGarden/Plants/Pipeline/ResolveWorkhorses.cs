using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;
using TrayGarden.Reception;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolveWorkhorses
{
  [UsedImplicitly]
  public virtual void Process(InitializePlantArgs args)
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
      Log.Debug("Plant {0} supports service delegation".FormatWith(args.PlantObject.GetType().FullName), this);
    }
    args.Workhorses = workhorses;
  }
}