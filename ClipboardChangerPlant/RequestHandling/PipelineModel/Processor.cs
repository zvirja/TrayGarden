using System.Drawing;
using ClipboardChangerV2.NotificationIcon;

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

    protected virtual void HandleError(ProcessorArgs args, Icon errorIcon)
    {
      NotifyIconManager.ActualManager.SetNewIcon(errorIcon);
      args.Abort = true;
    }
    protected virtual void HandleError(ProcessorArgs args)
    {
      HandleError(args, ErrorTrayIcon);
    }
  }
}
