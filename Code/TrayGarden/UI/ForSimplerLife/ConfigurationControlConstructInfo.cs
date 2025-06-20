﻿using System.Collections.Generic;
using TrayGarden.UI.Configuration;
using TrayGarden.UI.Configuration.EntryVM;

namespace TrayGarden.UI.ForSimplerLife;

public class ConfigurationControlConstructInfo
{
  public bool AllowReboot { get; set; }

  public string ConfigurationDescription { get; set; }

  public List<ConfigurationEntryBaseVM> ConfigurationEntries { get; set; }

  public bool EnableResetAllOption { get; set; }

  public ConfigurationControlVM ResultControlVM { get; set; }

  public void BuildControlVM()
  {
    ResultControlVM = new ConfigurationControlVM(ConfigurationEntries, EnableResetAllOption)
    {
      CalculateRebootOption =
        AllowReboot,
      ConfigurationDescription =
        ConfigurationDescription
    };
  }
}