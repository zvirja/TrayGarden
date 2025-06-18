﻿using JetBrains.Annotations;

namespace TrayGarden.Configuration.ApplicationConfiguration.GetApplicationConfigStepPipeline;

[UsedImplicitly]
public class AssignVisibleText
{
  public AssignVisibleText()
  {
    GlobalTitle = "Tray Garden -- Application configuration";
    Header = "Application configuration";
    ShortName = "app config";
    ConfigurationDescription = "Here you may configure global application settings. Some settings may require reboot to be applied";
  }

  public string ConfigurationDescription { get; set; }

  public string GlobalTitle { get; set; }

  public string Header { get; set; }

  public string ShortName { get; set; }

  [UsedImplicitly]
  public virtual void Process(GetApplicationConfigStepArgs args)
  {
    args.ConfigurationConstructInfo.ConfigurationDescription = ConfigurationDescription;
    var stepInfo = args.StepConstructInfo;
    stepInfo.GlobalTitle = GlobalTitle;
    stepInfo.Header = Header;
    stepInfo.ShortName = ShortName;
  }
}