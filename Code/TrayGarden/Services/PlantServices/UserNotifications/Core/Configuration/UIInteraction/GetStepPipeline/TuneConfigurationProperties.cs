﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.UI.ForSimplerLife;

namespace TrayGarden.Services.PlantServices.UserNotifications.Core.Configuration.UIInteraction.GetStepPipeline;

[UsedImplicitly]
public class TuneConfigurationProperties
{
  public TuneConfigurationProperties()
  {
    this.ConfigurationDescription = "This window allows to tune the User Nofications service properties";
  }

  public string ConfigurationDescription { get; set; }

  [UsedImplicitly]
  public virtual void Process(UNConfigurationStepArgs args)
  {
    ConfigurationControlConstructInfo constructInfo = args.ConfigurationConstructInfo;
    constructInfo.AllowReboot = false;
    constructInfo.EnableResetAllOption = true;
    constructInfo.ConfigurationDescription = this.ConfigurationDescription;
  }
}