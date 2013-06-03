using System;
using System.Collections.Generic;
using System.Xml;
using JetBrains.Annotations;
using TrayGarden.Configuration;
using TrayGarden.Diagnostics;
using TrayGarden.Helpers;

namespace TrayGarden.Pipelines.Engine
{
    [UsedImplicitly]
    public class PipelineManager : IPipelineManager
    {
        protected Dictionary<string, IPipeline> PipelinesInternal { get; set; }
        protected bool Initialized { get; set; }

        public PipelineManager()
        {
            PipelinesInternal = new Dictionary<string, IPipeline>();
        }

        [UsedImplicitly]
        public virtual void Initialize(IEnumerable<IPipeline> pipelines)
        {
            Assert.ArgumentNotNull(pipelines, "pipelines");
            PipelinesInternal = new Dictionary<string, IPipeline>();
            foreach (IPipeline pipeline in pipelines)
            {
                var pipelineKey = GetPipelineKey(pipeline.Name, pipeline.ArgumentType);
                PipelinesInternal.Add(pipelineKey, pipeline);
            }
            Initialized = true;
        }

        protected virtual string GetPipelineKey(string pipelineName, Type pipelineArgumentType)
        {
            return string.Format("{0}&&{1}", pipelineName, pipelineArgumentType);
        }

        public virtual void InvokePipeline<TArgumentType>([NotNull] string pipelineName,
                                                          [NotNull] TArgumentType argument)
            where TArgumentType : PipelineArgs
        {
            Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");
            Assert.ArgumentNotNull(argument, "argument");
            if (!Initialized)
                throw new NonInitializedException();
            IPipeline pipeline = ResolvePipeline(pipelineName, argument.GetType());
            if (pipeline == null)
                return;
            pipeline.Invoke(argument, true);
        }

        public virtual void InvokePipelineUnmaskedExceptions<TArgumentType>([NotNull] string pipelineName,
                                                                            [NotNull] TArgumentType argument)
            where TArgumentType : PipelineArgs
        {
            Assert.ArgumentNotNullOrEmpty(pipelineName, "pipelineName");
            Assert.ArgumentNotNull(argument, "argument");
            if (!Initialized)
                throw new NonInitializedException();
            IPipeline pipeline = ResolvePipeline(pipelineName, argument.GetType());
            Assert.IsNotNull(pipeline, "Can't resolve pipeline {0}".FormatWith(pipelineName));
            pipeline.Invoke(argument, false);
        }

        protected virtual IPipeline ResolvePipeline(string pipelineName, Type argumentType)
        {
            string key = GetPipelineKey(pipelineName, argumentType);
            if (!PipelinesInternal.ContainsKey(key))
                return null;
            IPipeline pipeline = PipelinesInternal[key];
            return pipeline;
        }
    }
}