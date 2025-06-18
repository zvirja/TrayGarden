using ClipboardChangerPlant.RequestHandling.PipelineModel;

namespace ClipboardChangerPlant.RequestHandling.Specialized;

public class OnlyShorteningHandler : RequestHandler
{
  public override bool? Match(ProcessorArgs args)
  {
    if (!args.OnlyShorteningRequired)
    {
      return false;
    }
    if (base.Match(args) == true)
    {
      return true;
    }
    else
    {
      return null;
    }
  }
}