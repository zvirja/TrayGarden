using System.Drawing;
using System.IO;

namespace TrayGarden.Resources
{
  public interface IResourcesManager
  {
    string GetStringResource(string resourceName, string defaultValue);
    Icon GetIconResource(string resourceName, Icon defaultValue);
    Bitmap GetBitmapResource(string resourceName, Bitmap defaultValue);
    T GetObjectResource<T>(string resourceName, T defaultValue) where T : class;
    Stream GetStream(string resourceName, Stream defaultValue);
  }
}