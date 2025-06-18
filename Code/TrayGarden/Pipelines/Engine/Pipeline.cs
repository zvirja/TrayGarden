using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    protected bool Initialized { get; set; }

    protected virtual List<Processor> Processors { get; set; }

    [UsedImplicitly]
    public virtual void Initialize([NotNull] Type argumentType, [NotNull] string name, List<Processor> processors)
    {
      //Assert.ArgumentNotNull(argumentType, "argumentTypeStr");
      Assert.ArgumentNotNull(name, "name");
      Assert.ArgumentNotNull(processors, "processors");
      this.Name = name;
      if (this.ArgumentType == null)
      {
        this.ArgumentType = argumentType;
      }
      if (this.ArgumentType == null)
      {
        throw new Exception("Pipeline {0}. Argument type invalid".FormatWith(name));
      }
      this.Processors = processors;
      this.Initialized = true;
    }

    public virtual void Invoke<TArgumentType>(TArgumentType argument, bool maskExceptions) where TArgumentType : PipelineArgs
    {
      if (!this.Initialized)
      {
        throw new NonInitializedException();
      }
      if (argument.GetType() != this.ArgumentType)
      {
        throw new ArgumentException(
          "This pipeline was designed to work with {0} type. Passed argument of type {1} was passed".FormatWith(
            this.ArgumentType.Name,
            argument.GetType().Name));
      }
      foreach (Processor processor in this.Processors)
      {
        try
        {
          processor.Invoke(argument);
          if (argument.Aborted)
          {
            break;
          }
        }
        catch (Exception ex)
        {
          Log.Error("Processor executing {0} error.".FormatWith(processor.ToString()), ex, this);
          if (maskExceptions)
          {
            break;
          }
          throw;
        }
      }
    }

    public override string ToString()
    {
      return this.Initialized
               ? "Processor {0}. ArgumentType: {1}, number of processors {2}".FormatWith(
                 this.Name,
                 this.ArgumentType.FullName,
                 this.Processors.Count)
               : base.ToString();
    }
  }
}