using Microsoft.Extensions.DependencyInjection;

using TrayGarden.Pipelines.Engine;

namespace TrayGarden.Composition;

public static class PipelineRegistrationExtensions
{
  public static PipelineBuilder<TArgs> AddPipeline<TArgs>(this IServiceCollection services)
    where TArgs : PipelineArgs
  {
    return new PipelineBuilder<TArgs>(services);
  }
}

public sealed class PipelineBuilder<TArgs>
  where TArgs : PipelineArgs
{
  private readonly IServiceCollection _services;

  internal PipelineBuilder(IServiceCollection services)
  {
    _services = services;
  }

  public PipelineBuilder<TArgs> Add<TProcessor>()
    where TProcessor : class, IPipelineProcessor<TArgs>
  {
    _services.AddTransient<IPipelineProcessor<TArgs>, TProcessor>();
    return this;
  }
}
