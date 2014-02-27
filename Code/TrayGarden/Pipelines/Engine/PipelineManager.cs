#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

#endregion

namespace TrayGarden.Pipelines.Engine
{
  [UsedImplicitly]
  public class PipelineManager : IPipelineManager
  {
    #region Constructors and Destructors

    public PipelineManager()
    {
      this.PipelinesInternal = new Dictionary<string, IPipeline>();
    }

    #endregion

    #region Properties

    protected bool Initialized { get; set; }

    protected Dictionary<string, IPipeline> PipelinesInternal { get; set; }

    #endregion

    #region Public Methods and Operators

    [UsedImplicitly]
    public virtual void Initialize(IEnumerable<IPipeline> pipelines)
    {
      Assert.ArgumentNotNull(pipelines, "pipelines");
      this.PipelinesInternal = new Dictionary<string, IPipeline>();
      foreach (IPipeline pipeline in pipelines)
      {
        var pipelineKey = this.GetPipelineKey(pipeline.Name, pipeline.ArgumentType);
        this.PipelinesInternal.Add(pipelineKey, pipeline);
      }
      this.Initialized = true;
    }

    public virtual void InvokePipeline<TArgumentType>([NotNull] string pipelineName, [NotNull] TArgumentType argument)
      where TArgumentType : PipelineArgs
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");
      Assert.ArgumentNotNull(argument, "argument");
      if (!this.Initialized)
      {
        throw new NonInitializedException();
      }
      IPipeline pipeline = this.ResolvePipeline(pipelineName, argument.GetType());
      if (pipeline == null)
      {
        return;
      }
      pipeline.Invoke(argument, true);
    }

    public virtual void InvokePipelineUnmaskedExceptions<TArgumentType>([NotNull] string pipelineName, [NotNull] TArgumentType argument)
      where TArgumentType : PipelineArgs
    {
      Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");
      Assert.ArgumentNotNull(argument, "argument");
      if (!this.Initialized)
      {
        throw new NonInitializedException();
      }
      IPipeline pipeline = this.ResolvePipeline(pipelineName, argument.GetType());
      Assert.IsNotNull(pipeline, "Can't resolve pipeline {0}".FormatWith(pipelineName));
      pipeline.Invoke(argument, false);
    }

    #endregion

    #region Methods

    protected virtual string GetPipelineKey(string pipelineName, Type pipelineArgumentType)
    {
      return string.Format("{0}&&{1}", pipelineName, pipelineArgumentType);
    }

    protected virtual IPipeline ResolvePipeline(string pipelineName, Type argumentType)
    {
      string key = this.GetPipelineKey(pipelineName, argumentType);
      if (!this.PipelinesInternal.ContainsKey(key))
      {
        return null;
      }
      IPipeline pipeline = this.PipelinesInternal[key];
      return pipeline;
    }

    #endregion
  }
}