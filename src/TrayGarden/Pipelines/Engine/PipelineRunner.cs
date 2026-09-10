using System;

using Microsoft.Extensions.DependencyInjection;

using TrayGarden.Diagnostics;

namespace TrayGarden.Pipelines.Engine;

public class PipelineRunner : IPipelineRunner
{
  private readonly IServiceProvider _serviceProvider;

  public PipelineRunner(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public void Run<TArgs>(TArgs args, bool maskExceptions = true) where TArgs : PipelineArgs
  {
    Assert.ArgumentNotNull(args, "args");
    foreach (IPipelineProcessor<TArgs> processor in _serviceProvider.GetServices<IPipelineProcessor<TArgs>>())
    {
      try
      {
        processor.Process(args);
        if (args.Aborted)
        {
          break;
        }
      }
      catch (Exception ex)
      {
        Log.For(this).Error(
          ex,
          "Processor {Processor} failed in pipeline {Pipeline}",
          processor.GetType().FullName,
          typeof(TArgs).Name);
        if (maskExceptions)
        {
          break;
        }
        throw;
      }
    }
  }
}
