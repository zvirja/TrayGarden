using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline;

[UsedImplicitly]
public class StoreOriginalUrlValue : Processor
{
  public override void Process(ProcessorArgs args)
  {
    args.OriginalUrl = args.ResultUrl;
  }
}