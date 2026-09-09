namespace TrayGarden.Pipelines.Engine;

public interface IPipelineRunner
{
  void Run<TArgs>(TArgs args, bool maskExceptions = true) where TArgs : PipelineArgs;
}
