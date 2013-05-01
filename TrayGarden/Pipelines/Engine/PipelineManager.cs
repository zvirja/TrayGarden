using System;
using System.Collections.Generic;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.Pipelines.Engine
{
    public class PipelineManager : IRequireInitialization, IPipelineManager
    {
        protected Dictionary<string, IPipeline> PipelinesInternal { get; set; }
        public List<IPipeline> Pipelines { get; set; }

        public PipelineManager()
        {
            Pipelines = new List<IPipeline>();
            PipelinesInternal = new Dictionary<string, IPipeline>();
        }

        public virtual void Initialize()
        {
            foreach (IPipeline pipeline in Pipelines)
            {
                var pipelineKey = GetPipelineKey(pipeline.Name, pipeline.ArgumentType);
                PipelinesInternal.Add(pipelineKey, pipeline);
            }
        }

        protected virtual string GetPipelineKey(string pipelineName, Type pipelineArgumentType)
        {
            return string.Format("{0}&&{1}", pipelineName, pipelineArgumentType);
        }

        public virtual void InvokePipeline<TArgumentType>(string pipelineName, TArgumentType argument)
            where TArgumentType : PipelineArgs
        {
            string key = GetPipelineKey(pipelineName, argument.GetType());
            if (!PipelinesInternal.ContainsKey(key))
                return;
            IPipeline pipeline = PipelinesInternal[key];
            pipeline.Invoke(argument);
        }
    }
}