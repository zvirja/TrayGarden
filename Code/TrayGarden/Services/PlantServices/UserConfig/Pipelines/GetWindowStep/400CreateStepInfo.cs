using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JetBrains.Annotations;

using TrayGarden.Diagnostics;
using TrayGarden.UI.ForSimplerLife;
using TrayGarden.UI.WindowWithReturn;

namespace TrayGarden.Services.PlantServices.UserConfig.Pipelines.GetWindowStep;

[UsedImplicitly]
public class CreateStepInfo
{
  public CreateStepInfo()
  {
    this.ShortName = "user settings";
    this.Header = "User settings for plant";
    this.GlobalTitle = "Tray Garden -- User settings for #plantName plant";
  }

  public string GlobalTitle { get; set; }

  public string Header { get; set; }

  public string ShortName { get; set; }

  [UsedImplicitly]
  public virtual void Process(GetUCStepPipelineArgs args)
  {
    Assert.IsNotNull(args.ConfigurationConstructInfo.ResultControlVM, "args.ConfigurationConstructInfo.ResultControlVM");
    WindowWithBackStateConstructInfo stateInfo = args.StateConstructInfo;
    stateInfo.ResultState = new WindowStepState(
      this.GetGlobalTitle(args),
      stateInfo.Header ?? this.Header,
      stateInfo.ShortName ?? this.ShortName,
      args.ConfigurationConstructInfo.ResultControlVM,
      stateInfo.SuperAction,
      stateInfo.StateSpecificHelpActions);
  }

  protected virtual string GetGlobalTitle(GetUCStepPipelineArgs args)
  {
    var globalTitle = args.StateConstructInfo.GlobalTitle ?? this.GlobalTitle;
    return globalTitle.Replace("#plantName", args.UCServicePlantBox.RelatedPlantEx.Plant.HumanSupportingName);
  }
}