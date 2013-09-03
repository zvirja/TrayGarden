using System.Drawing;
using ClipboardChangerPlant.NotificationIcon;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel
{
  public class Processor
  {
    public virtual void Process(ProcessorArgs args) { }


    protected virtual Icon NotFoundTrayIcon
    {
      get { return NotifyIconManager.ActualManager.NotFoundTrayIcon; }
    }

    protected virtual Icon ErrorTrayIcon
    {
      get { return NotifyIconManager.ActualManager.ErrorTrayIcon; }
    }

    protected virtual Icon SuccessTrayIcon
    {
      get { return NotifyIconManager.ActualManager.SuccessTrayIcon; }
    }

    protected virtual void HandleErrorAndAbortPipeline(ProcessorArgs args, Icon errorIcon)
    {
      args.CurrentNotifyIconChangerClient.SetIcon(errorIcon);
      args.Abort();
    }

    protected virtual void HandleErrorAndAbortPipeline(ProcessorArgs args)
    {
      HandleErrorAndAbortPipeline(args, ErrorTrayIcon);
    }
  }
}
