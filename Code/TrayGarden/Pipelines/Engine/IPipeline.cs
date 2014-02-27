#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace TrayGarden.Pipelines.Engine
{
  public interface IPipeline
  {
    #region Public Properties

    Type ArgumentType { get; }

    string Name { get; }

    #endregion

    #region Public Methods and Operators

    void Invoke<TArgumentType>(TArgumentType argument, bool maskExceptions) where TArgumentType : PipelineArgs;

    #endregion
  }
}