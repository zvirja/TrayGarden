using System;
using System.Drawing;

namespace TrayGarden.Resources
{
    internal class GlobalResourcesManager
    {
        public static Icon GetIconByName(string name)
        {
            return (Icon) GlobalResources.ResourceManager.GetObject(name);
        }

        public static String GetStringByName(string name)
        {
            return GlobalResources.ResourceManager.GetString(name);
        }
    }
}