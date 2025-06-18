using ClipboardChangerPlant.Configuration;

using JetBrains.Annotations;

using TrayGarden.Helpers;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel.Pipeline;

[UsedImplicitly]
public class PullValueFromClipboard : Processor
{
  public override void Process(ProcessorArgs args)
  {
    if (args.ClipboardEvent)
    {
      if (args.PredefinedClipboardValue.IsNullOrEmpty())
      {
        args.Abort();
        return;
      }
      args.ResultUrl = args.PredefinedClipboardValue;
    }
    else
    {
      string currentValue = Factory.ActualFactory.GetClipboardProvider().GetValue();
      if (currentValue.IsNullOrEmpty())
      {
        this.HandleErrorAndAbortPipeline(args, this.NotFoundTrayIcon);
      }
      else
      {
        args.ResultUrl = currentValue;
      }
    }
  }
}