namespace TrayGarden.Pipelines.Engine;

public interface IPipelineProcessor<in TArgs> where TArgs : PipelineArgs
{
  void Process(TArgs args);
}
