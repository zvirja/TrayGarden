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
    public virtual void Initialize([NotNull] Type argumentType, [NotNull] string name, List<Processor> processors)
    {
      //Assert.ArgumentNotNull(argumentType, "argumentTypeStr");
      Assert.ArgumentNotNull(name, "name");
      Assert.ArgumentNotNull(processors, "processors");
      Name = name;
      if (ArgumentType == null)
        ArgumentType = argumentType;
      if (ArgumentType == null)
        throw new Exception("Pipeline {0}. Argument type invalid".FormatWith(name));
      Processors = processors;
      Initialized = true;
    }

    public virtual void Invoke<TArgumentType>(TArgumentType argument, bool maskExceptions)
        where TArgumentType : PipelineArgs
    {
      if (!Initialized)
        throw new NonInitializedException();
      if (argument.GetType() != ArgumentType)
        throw new ArgumentException(
            "This pipeline was designed to work with {0} type. Passed argument of type {1} was passed"
                .FormatWith(ArgumentType.Name, argument.GetType().Name));
      foreach (Processor processor in Processors)
      {
        try
        {
          processor.Invoke(argument);
          if (argument.Aborted)
            break;
        }
        catch (Exception ex)
        {
          Log.Error("Processor executing {0} error.".FormatWith(processor.ToString()), ex, this);
          if (maskExceptions)
            break;
          throw;
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