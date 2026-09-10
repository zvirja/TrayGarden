using System.Drawing;
using System.IO;

namespace TrayGarden.Resources;

public interface IResourcesManager
{
  Bitmap GetBitmapResource(string resourceName, Bitmap defaultValue);

  Icon GetIconResource(string resourceName, Icon defaultValue);

  T GetObjectResource<T>(string resourceName, T defaultValue) where T : class;

  Stream GetStream(string resourceName, Stream defaultValue);

  string GetStringResource(string resourceName, string defaultValue);
}