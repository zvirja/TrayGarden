using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.Helpers;

namespace TrayGarden.Pipelines.Engine
{
    [UsedImplicitly]
    public class Pipeline : IPipeline
    {
        public virtual Type ArgumentType { get; protected set; }
        public virtual string Name { get; protected set; }
        protected virtual List<Processor> Processors { get; set; }
        protected bool Initialized { get; set; }

        [UsedImplicitly]
        public virtual void Initialize(string argumentTypeStr,string name,List<Processor> processors )
        {
            if (processors == null) throw new ArgumentNullException("processors");
            if (argumentTypeStr.IsNullOrEmpty()) throw new ArgumentNullException("argumentTypeStr");
            if (name.IsNullOrEmpty()) throw new ArgumentNullException("name");
            Name = name;
            if (ArgumentType == null)
                ArgumentType = ReflectionHelper.ResolveType(argumentTypeStr);
            if (ArgumentType == null)
                throw new Exception("Pipeline. Argument type invalid");
            Processors = processors;
            Initialized = true;
        }

        public virtual void Invoke<TArgumentType>(TArgumentType argument) where TArgumentType : PipelineArgs
        {
            if(!Initialized)
                throw new NonInitializedException();
            foreach (Processor processor in Processors)
            {
                processor.Invoke(argument);
                if (argument.Aborted)
                    break;
            }
        }
    }
}