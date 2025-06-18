using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

namespace TrayGarden.Pipelines.Engine;

public class PipelineArgs
{
  public bool Aborted { get; protected set; }

  public object Result { get; set; }

  [UsedImplicitly]
  public virtual void Abort()
  {
    this.Aborted = true;
  }
}