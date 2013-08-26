using JetBrains.Annotations;

namespace TrayGarden.Pipelines.Engine
{
  public interface IPipelineManager
  {
    void InvokePipeline<TArgumentType>(string pipelineName, TArgumentType argument)
        where TArgumentType : PipelineArgs;

    void InvokePipelineUnmaskedExceptions<TArgumentType>([NotNull] string pipelineName,
                                                         [NotNull] TArgumentType argument)
         where TArgumentType : PipelineArgs;
  }
}