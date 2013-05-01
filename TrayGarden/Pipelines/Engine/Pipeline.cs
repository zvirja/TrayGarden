using System;
using System.Collections.Generic;
using System.Xml;
using TrayGarden.Configuration;
using TrayGarden.Helpers;

namespace TrayGarden.Pipelines.Engine
{
    public class Pipeline : IRequireInitialization, IPipeline
    {
        public virtual Type ArgumentType { get; set; }
        public virtual string ArgumentTypeStr { get; set; }
        public virtual string Name { get; set; }
        public virtual List<object> ProcessorObjects { get; set; }
        protected virtual List<Processor> Processors { get; set; }

        public Pipeline()
        {
            ProcessorObjects = new List<object>();
            Processors = new List<Processor>();
        }


        public virtual void Invoke<TArgumentType>(TArgumentType argument) where TArgumentType : PipelineArgs
        {
            foreach (Processor processor in Processors)
            {
                processor.Invoke(argument);
                if (argument.Aborted)
                    break;
            }
        }

        public virtual void Initialize()
        {
            if (ArgumentType == null)
                ArgumentType = ReflectionHelper.ResolveType(ArgumentTypeStr);
            if (ArgumentType == null)
                return;
            foreach (object processorObject in ProcessorObjects)
            {
                var processor = new Processor(processorObject, ArgumentType);
                Processors.Add(processor);
            }
        }
    }
}