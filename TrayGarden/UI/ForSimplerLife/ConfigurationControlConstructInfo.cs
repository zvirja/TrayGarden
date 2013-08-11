﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.Stuff;

namespace TrayGarden.UI.ForSimplerLife
{
    public class ConfigurationControlConstructInfo
    {
        public List<ConfigurationEntryVMBase> ConfigurationEntries { get; set; }
        public string ConfigurationDescription { get; set; }
        public bool AllowResetOption { get; set; }
        public bool AllowReboot { get; set; }

        public ConfigurationControlVM ResultControlVM { get; set; }
    }
}
