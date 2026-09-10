using System.Drawing;
using ClipboardChangerPlant.NotificationIcon;

namespace ClipboardChangerPlant.RequestHandling.PipelineModel;

public class Processor
{
  protected Icon ErrorTrayIcon
  {
    get
    {
      return NotifyIconManager.ActualManager.ErrorTrayIcon;
    }
  }

  protected Icon NotFoundTrayIcon
  {
    get
    {
      return NotifyIconManager.ActualManager.NotFoundTrayIcon;
    }
  }

  protected Icon SuccessTrayIcon
  {
    get
    {
      return NotifyIconManager.ActualManager.SuccessTrayIcon;
    }
  }

  public virtual void Process(ProcessorArgs args)
  {
  }

  protected void HandleErrorAndAbortPipeline(ProcessorArgs args, Icon errorIcon)
  {
    args.CurrentNotifyIconChangerClient.SetIcon(errorIcon);
    args.Abort();
  }

  private void HandleErrorAndAbortPipeline(ProcessorArgs args)
  {
    HandleErrorAndAbortPipeline(args, ErrorTrayIcon);
  }
}