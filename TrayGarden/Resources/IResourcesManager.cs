using System.Drawing;

namespace TrayGarden.Resources
{
    public interface IResourcesManager
    {
        string GetStringResource(string resourceName, string defaultValue);
        Icon GetIconResource(string resourceName, Icon defaultValue);
    }
}