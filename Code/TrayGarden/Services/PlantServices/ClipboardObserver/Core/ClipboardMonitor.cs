using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core
{
  public class ClipboardMonitor: Form
  {
    public event EventHandler ClipboardValueChanged;

    public ClipboardMonitor()
    {
      NativeHelper.SetParent(Handle, NativeHelper.HWND_MESSAGE);
      NativeHelper.AddClipboardFormatListener(Handle);
    }

    protected override void WndProc(ref Message m)
    {
      if(m.Msg == NativeHelper.WM_CLIPBOARDUPDATE)
        OnClipboardValueChanged();
      base.WndProc(ref m);
    }

    protected virtual void OnClipboardValueChanged()
    {
      EventHandler handler = ClipboardValueChanged;
      if (handler != null) handler(this, EventArgs.Empty);
    }
  }
}
