using System;
using System.Runtime.InteropServices;

namespace TrayGarden.Helpers;

public static class NativeHelper
{
  // See http://msdn.microsoft.com/en-us/library/ms649021%28v=vs.85%29.aspx

  public const int WM_CLIPBOARDUPDATE = 0x031D;

  public static IntPtr HWND_MESSAGE = new IntPtr(-3);

  // See http://msdn.microsoft.com/en-us/library/ms649033%28VS.85%29.aspx

  [DllImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  public static extern bool AddClipboardFormatListener(IntPtr hwnd);

  [DllImport("gdi32.dll")]
  public static extern bool DeleteObject(IntPtr hObject);

  // See http://msdn.microsoft.com/en-us/library/ms633541%28v=vs.85%29.aspx
  // See http://msdn.microsoft.com/en-us/library/ms632599%28VS.85%29.aspx#message_only
  [DllImport("user32.dll", SetLastError = true)]
  public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
}