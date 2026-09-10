using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace MicrosoftTodoTrayPlant;

internal static class PlantResources
{
  private const string IconResourceName = "MicrosoftTodoTrayPlant.Resources.todo.ico";

  public static Icon LoadTrayIcon()
  {
    using Stream stream = OpenIconStream();
    return new Icon(stream);
  }

  public static BitmapFrame LoadWindowIcon()
  {
    using Stream stream = OpenIconStream();
    return BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
  }

  private static Stream OpenIconStream()
  {
    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(IconResourceName);
    if (stream == null)
    {
      throw new InvalidOperationException($"Embedded resource '{IconResourceName}' was not found.");
    }

    return stream;
  }
}
