using JetBrains.Annotations;

namespace TrayGarden.Pipelines.Engine;

public class PipelineArgs
{
  public bool Aborted { get; private set; }

  public object Result { get; set; }

  [UsedImplicitly]
  public void Abort()
  {
    Aborted = true;
  }
}