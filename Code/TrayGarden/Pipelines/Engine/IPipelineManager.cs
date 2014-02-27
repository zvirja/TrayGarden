#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

#endregion

namespace TrayGarden.Pipelines.Engine
{
  public interface IPipelineManager
  {
    #region Public Methods and Operators

    void InvokePipeline<TArgumentType>(string pipelineName, TArgumentType argument) where TArgumentType : PipelineArgs;

    void InvokePipelineUnmaskedExceptions<TArgumentType>([NotNull] string pipelineName, [NotNull] TArgumentType argument)
      where TArgumentType : PipelineArgs;

    #endregion
  }
}