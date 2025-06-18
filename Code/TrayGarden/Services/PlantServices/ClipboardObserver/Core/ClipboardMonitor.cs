using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using TrayGarden.Helpers;

namespace TrayGarden.Services.PlantServices.ClipboardObserver.Core;

public class ClipboardMonitor : Form
{
  public ClipboardMonitor()
  {
    NativeHelper.SetParent(this.Handle, NativeHelper.HWND_MESSAGE);
    NativeHelper.AddClipboardFormatListener(this.Handle);
  }

  public event EventHandler ClipboardValueChanged;

  protected virtual void OnClipboardValueChanged()
  {
    EventHandler handler = this.ClipboardValueChanged;
    if (handler != null)
    {
      handler(this, EventArgs.Empty);
    }
  }

  protected override void WndProc(ref Message m)
  {
    if (m.Msg == NativeHelper.WM_CLIPBOARDUPDATE)
    {
      this.OnClipboardValueChanged();
    }
    base.WndProc(ref m);
  }
}