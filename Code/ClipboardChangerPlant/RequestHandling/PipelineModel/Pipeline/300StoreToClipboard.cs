using ClipboardChangerPlant.Clipboard;

using JetBrains.Annotations;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline;

[UsedImplicitly]
public class StoreToClipboard : Processor
{
  public override void Process(ProcessorArgs args)
  {
    ClipboardManager.SetValue(args.ResultUrl, true);
  }
}