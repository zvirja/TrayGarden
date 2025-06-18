﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.UI.ForSimplerLife
{
  public class ConfigurationControlConstructInfo
  {
    public bool AllowReboot { get; set; }

    public string ConfigurationDescription { get; set; }

    public List<ConfigurationEntryBaseVM> ConfigurationEntries { get; set; }

    public bool EnableResetAllOption { get; set; }

    public ConfigurationControlVM ResultControlVM { get; set; }

    public void BuildControlVM()
    {
      this.ResultControlVM = new ConfigurationControlVM(this.ConfigurationEntries, this.EnableResetAllOption)
                               {
                                 CalculateRebootOption =
                                   this.AllowReboot,
                                 ConfigurationDescription =
                                   this
                                   .ConfigurationDescription
                               };
    }
  }
}