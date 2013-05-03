using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.Configuration;

namespace TrayGarden.Configuration
{
    public static class Settings
    {
        public static string ApplicationDataFolderName
        {
            get { return Factory.Instance.GetStringSetting("ApplicationData.FolderName", "TrayGarden"); }
        }
    }
}