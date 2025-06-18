using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.Plants.Pipeline;

[UsedImplicitly]
public class ResolvePlantID
{
  [UsedImplicitly]
  public virtual void Process(InitializePlantArgs args)
  {
    args.PlantID = args.PlantObject.GetType().FullName;
  }
}