#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

#endregion

namespace TrayGarden.Helpers
{
  public static class ImageHelper
  {
    #region Public Methods and Operators

    public static BitmapSource Bitmap2BitmapImage(Bitmap bitmap)
    {
      IntPtr hBitmap = bitmap.GetHbitmap(System.Drawing.Color.Aqua);
      BitmapSource retval;

      try
      {
        retval = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
      }
      finally
      {
        NativeHelper.DeleteObject(hBitmap);
      }

      return retval;
    }

    public static BitmapImage GetBitmapImageFromBitmapThreadSafe(Bitmap bitmap, ImageFormat imageFormat)
    {
      if (bitmap == null)
      {
        return null;
      }
      var memoryStream = new MemoryStream();
      bitmap.Save(memoryStream, imageFormat);
      memoryStream.Position = 0;

      var result = new BitmapImage();
      result.BeginInit();
      result.StreamSource = memoryStream;
      result.CacheOption = BitmapCacheOption.OnLoad;
      result.EndInit();
      memoryStream.Close();
      result.Freeze();
      return result;
    }

    #endregion
  }
}