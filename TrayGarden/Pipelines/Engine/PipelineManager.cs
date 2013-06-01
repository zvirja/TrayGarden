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

        public virtual void InvokePipeline<TArgumentType>(string pipelineName, TArgumentType argument)
            where TArgumentType : PipelineArgs
        {
            if(!Initialized)
                throw new NonInitializedException();
            string key = GetPipelineKey(pipelineName, argument.GetType());
            if (!PipelinesInternal.ContainsKey(key))
                return;
            IPipeline pipeline = PipelinesInternal[key];
            pipeline.Invoke(argument);
        }
    }
}