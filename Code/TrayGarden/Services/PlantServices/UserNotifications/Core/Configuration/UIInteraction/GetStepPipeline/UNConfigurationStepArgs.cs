﻿using System.Collections.Generic;
using TrayGarden.Pipelines.Engine;
using TrayGarden.UI.Configuration.EntryVM;
using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

public class UNConfigurationStepArgs : PipelineArgs
{
  public UNConfigurationStepArgs()
  {
    StateConstructInfo = new WindowWithBackStateConstructInfo();
    ConfigurationConstructInfo = new ConfigurationControlConstructInfo()
    {
      ConfigurationEntries =
        new List<ConfigurationEntryBaseVM>()
    };
  }

  public ConfigurationControlConstructInfo ConfigurationConstructInfo { get; set; }

  public WindowWithBackStateConstructInfo StateConstructInfo { get; set; }
}