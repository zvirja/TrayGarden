using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TrayGarden.Diagnostics;
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
        public virtual void Initialize([NotNull] string argumentTypeStr, [NotNull] string name,List<Processor> processors )
        {
            Assert.ArgumentNotNull(argumentTypeStr, "argumentTypeStr");
            Assert.ArgumentNotNull(name, "name");
            Assert.ArgumentNotNull(processors, "processors");
            Name = name;
            if (ArgumentType == null)
                ArgumentType = ReflectionHelper.ResolveType(argumentTypeStr);
            if (ArgumentType == null)
                throw new Exception("Pipeline {0}. Argument type invalid".FormatWith(name));
            Processors = processors;
            Initialized = true;
        }

        public virtual void Invoke<TArgumentType>(TArgumentType argument) where TArgumentType : PipelineArgs
        {
            if(!Initialized)
                throw new NonInitializedException();
            foreach (Processor processor in Processors)
            {
                try
                {
                    processor.Invoke(argument);
                    if (argument.Aborted)
                        break;
                }
                catch (Exception e)
                {
                    Log.Error("Processor executing {0} error.".FormatWith(processor.ToString()));
                    break;
                }
            }
        }

        public override string ToString()
        {
            return Initialized
                       ? "Processor {0}. ArgumentType: {1}, number of processors {2}".FormatWith(Name,
                                                                                                 ArgumentType.FullName,
                                                                                                 Processors.Count)
                       : base.ToString();
        }
    }
}