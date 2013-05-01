using System.Xml;
using TrayGarden.Helpers;

namespace TrayGarden.Configuration
{
    interface INeedConfigurationNode
    {
        void SetConfigurationNode(XmlNode configurationNode);
    }
}
