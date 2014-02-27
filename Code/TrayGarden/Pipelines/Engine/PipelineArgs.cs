#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Pipelines.Engine
{
  public class PipelineArgs
  {
    #region Public Properties

    public bool Aborted { get; protected set; }

    public object Result { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Abort()
    {
      this.Aborted = true;
    }

    #endregion
  }
}